using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Auth;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.Interfaces;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Skill_Exchange.Infrastructure.AuthenticationServices
{
    public class AuthService : IAuthService
    {
        // ================================
        // Dependencies
        // ================================
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordHasher<AppUser> _passwordHasher = new();
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, IMapper mapper, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _emailService = emailService;
            _mapper = mapper;
        }

        // ================================
        // REGISTRATION
        // ================================
        public async Task<Result<bool>> StartRegisterAsync(string email)
        {
            // Check if user already exists
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user != null)
                return Result<bool>.Fail("User already exists");

            // Generate verification code
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            string verificationCode = Convert.ToBase64String(randomBytes);

            // If a pending verification exists → update it
            var pending = await _unitOfWork.PendingVerifications.GetByEmailAsync(email);
            if (pending != null)
            {
                pending.VerificationCode = verificationCode;
                pending.Expiry = DateTime.UtcNow.AddMinutes(30);
                await _unitOfWork.PendingVerifications.UpdateAsync(pending);
                await _unitOfWork.CompleteAsync();

                var sent = await _emailService.SendEmailAsync(email, "Email Verification", "verification code", verificationCode);
                return sent ? Result<bool>.Ok(true) : Result<bool>.Fail("Failed to send verification email.");
            }

            // Otherwise create new pending verification
            var isSent = await _emailService.SendEmailAsync(email, "Email Verification", "verification code", verificationCode);
            if (!isSent)
                return Result<bool>.Fail("Failed to send verification email.");

            var pendingVerification = new PendingVerification
            {
                Email = email,
                VerificationCode = verificationCode,
                Expiry = DateTime.UtcNow.AddMinutes(30),
                IsConfirmed = false
            };
            await _unitOfWork.PendingVerifications.AddAsync(pendingVerification);
            await _unitOfWork.CompleteAsync();

            return Result<bool>.Ok(true);
        }

        public async Task<bool> ConfirmEmailAsync(string verificationCode)
        {
            try
            {
                var pendingVerification = await _unitOfWork.PendingVerifications.GetByVerificationCodeAsync(verificationCode);
                if (pendingVerification == null || pendingVerification.Expiry < DateTime.UtcNow)
                    return false;

                // Mark as confirmed
                pendingVerification.IsConfirmed = true;
                await _unitOfWork.PendingVerifications.UpdateAsync(pendingVerification);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<RegisterResponseDto> CompleteRegisterAsync(CreateUserDTO createRequestDTO)
        {
            // Check if pending verification exists
            var PendingVerification = await _unitOfWork.PendingVerifications.GetByEmailAsync(createRequestDTO.Email);
            if (PendingVerification == null)
                return new RegisterResponseDto { Message = "You Must Register First" };

            if (!PendingVerification.IsConfirmed)
                return new RegisterResponseDto { Message = "Mail is not Confirmed" };

            // Map DTO → Entity and hash password
            var user = _mapper.Map<AppUser>(createRequestDTO);
            user.PasswordHash = _passwordHasher.HashPassword(user, createRequestDTO.Password);
            user.EmailConfirmed = true;

            // Save new user
            var is_user_created = await _unitOfWork.Users.AddAsync(user);
            if (!is_user_created)
                return new RegisterResponseDto { Message = "Regsiteration Failed!" };

            // Remove pending verification and save
            await _unitOfWork.PendingVerifications.DeleteAsync(PendingVerification);
            await _unitOfWork.CompleteAsync();

            return new RegisterResponseDto { Message = "Registeration succeded!" };
        }

        

        // ================================
        // LOGIN / LOGOUT
        // ================================
        public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            // Get user by email
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user == null)
                return Result<LoginResponseDto>.Fail("Invalid email or password.");

            // Check if email is confirmed
            if (!user.EmailConfirmed)
                return Result<LoginResponseDto>.Fail("This Email is not Confirmed.");

            // Verify password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                return Result<LoginResponseDto>.Fail("Invalid email or password.");

            // Update user and save changes
            _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            // Generate response with tokens
            var loginResponse = await GenerateLoginResponseAsync(user);
            return Result<LoginResponseDto>.Ok(loginResponse);
        }

        public Task LogoutAsync(Guid userId)
        {
            // To be implemented later
            throw new NotImplementedException();
        }

        // ================================
        // GOOGLE AUTH
        // ================================
        public async Task<RegisterResponseDto> GoogleSignupAsync(GoogleSignupRequestDto request)
        {
            // Check if user exists
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user != null)
                return new RegisterResponseDto { Message = "User with this email already exists." };

            // Validate Google token
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

            // Create new user from Google payload
            var newUser = new AppUser
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                EmailConfirmed = true
            };

            var is_user_created = await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.CompleteAsync();
            if (!is_user_created)
                return new RegisterResponseDto { Message = "Regsiteration Failed!" };

            return new RegisterResponseDto { Message = "Registeration succeded!" };
        }

        public async Task<LoginResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            var user = await _unitOfWork.Users.GetByEmailAsync(payload.Email);
            if (user == null)
                throw new Exception("User not found. Please signup with Google first.");

            return await GenerateLoginResponseAsync(user);
        }

        // ================================
        // PASSWORD MANAGEMENT
        // ================================
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
                return false;

            // Generate reset link
            var Id = user.Id;
            var link = $"https://yourfrontend.com/reset-password?token={Id}";

            var is_sent = await _emailService.SendEmailAsync(email, "Reset Password", "click here : ", link);
            return is_sent;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            try
            {
                // Token here = userId
                var user = await _unitOfWork.Users.GetByIdAsync(request.Token);
                if (user == null)
                    return false;

                user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch
            {
                throw new Exception("Reset password failed");
            }
        }

        public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequestDto request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
                return Result<bool>.Fail("User not found");

            // Verify old password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.OldPassword);
            if (result == PasswordVerificationResult.Failed)
                return Result<bool>.Fail("Current password is invalid");

            if (string.IsNullOrEmpty(request.NewPassword))
                return Result<bool>.Fail("New password is invalid");

            // Update with new password
            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return Result<bool>.Ok(true);
        }

        // ================================
        // TOKEN REFRESH
        // ================================
        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
                throw new Exception("Invalid token");

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId!));
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            // Generate new tokens
            var newAccessToken = _jwtService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60)
            };
        }

        // ================================
        // HELPERS
        // ================================
        private async Task<LoginResponseDto> GenerateLoginResponseAsync(AppUser user)
        {
            // Build claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}")
            };

            // Generate tokens
            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Update user with refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                Userinfo = _mapper.Map<UserDTO>(user)
            };
        }

        private string GenerateToken()
        {
            // Generate random secure token
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
