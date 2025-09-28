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
        private readonly UserManager<AppUser> _userManager;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, IMapper mapper, IEmailService emailService, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _emailService = emailService;
            _mapper = mapper;
            _userManager = userManager;
        }

        // ================================
        // REGISTRATION
        // ================================
        public async Task<Result<bool>> StartRegisterAsync(string email)
        {
            // 1. Check if user already exists
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                return Result<bool>.Fail("User already exists");

            // 2. Generate secure verification code
            string verificationCode = GenerateVerificationCode();

            // 3. Get or create pending verification
            var pending = await _unitOfWork.PendingVerifications.GetByEmailAsync(email);
            if (pending == null)
            {
                pending = new PendingVerification
                {
                    Email = email,
                    IsConfirmed = false
                };
                await _unitOfWork.PendingVerifications.AddAsync(pending);
            }

            // Always refresh code + expiry
            pending.VerificationCode = verificationCode;
            pending.Expiry = DateTime.UtcNow.AddMinutes(30);

            await _unitOfWork.PendingVerifications.UpdateAsync(pending);
            await _unitOfWork.CompleteAsync();

            // 4. Build email body
            var emailBody = $@"
<div style='font-family:Arial, sans-serif; max-width:600px; margin:auto; background:#f8f9fa; padding:30px; border-radius:10px;'>
    <div style='text-align:center; margin-bottom:20px;'>
        <img src='https://yourcdn.com/logo.png' alt='Skill Exchange' style='width:120px;'>
    </div>
    <h2 style='color:#2c3e50; text-align:center;'>Welcome to Skill Exchange!</h2>
    <p style='color:#555;'>Hi there,</p>
    <p style='color:#555;'>To complete your registration, please use the verification code below:</p>
    <div style='background:#3498db; padding:20px; text-align:center; border-radius:8px; margin:20px 0;'>
        <span style='color:#fff; font-size:24px; font-weight:bold; letter-spacing:3px;'>{verificationCode}</span>
    </div>
    <p style='color:#555;'>This code will expire in <strong>30 minutes</strong>.</p>
    <p style='color:#888; font-size:12px;'>If you did not request this, please ignore this email.</p>
    <hr style='border:none; border-top:1px solid #eee; margin:20px 0;'/>
    <p style='color:#888; font-size:12px; text-align:center;'>Thanks,<br/>The Skill Exchange Team</p>
</div>
";


            // 5. Send email
            var sent = await _emailService.SendEmailAsync(
                email,
                "Skill Exchange - Email Verification",
                emailBody,
                verificationCode
            );

            // 6. Return result
            return sent
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail("Failed to send verification email.");
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
            user.UserName = createRequestDTO.Email;
            user.EmailConfirmed = true;

            // Save new user
            var result = await _userManager.CreateAsync(user, createRequestDTO.Password);
            if (!result.Succeeded)
                return new RegisterResponseDto { Message = "Regsiteration Failed!" };

            await _userManager.AddToRoleAsync(user, "User");

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
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<LoginResponseDto>.Fail("Invalid email or password.");

            // Check if email is confirmed
            if (!user.EmailConfirmed)
                return Result<LoginResponseDto>.Fail("This Email is not Confirmed.");

            // Verify password
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                return Result<LoginResponseDto>.Fail("Invalid email or password.");

            // Update user and save changes
            await _unitOfWork.Users.UpdateAsync(user);
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
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
                return new RegisterResponseDto { Message = "User with this email already exists." };

            // Validate Google token
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

            // Create new user from Google payload  
            System.Console.WriteLine($"payload = {payload.Email}");
            System.Console.WriteLine($"email = {request.Email}");
            var newUser = new AppUser
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                EmailConfirmed = true,
                UserName = request.Email
            };

            var res = await _unitOfWork.Users.AddAsync(newUser);
            //var result = await _userManager.CreateAsync(newUser);

            // System.Console.WriteLine($"res = {result}");
            if (!res)
                return new RegisterResponseDto { Message = "Regsiteration Failed!" };
            await _unitOfWork.CompleteAsync();

            //await _userManager.AddToRoleAsync(newUser, "User");
            return new RegisterResponseDto { Message = "Registeration succeded!" };
        }

        public async Task<LoginResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
                throw new Exception("User not found. Please signup with Google first.");

            return await GenerateLoginResponseAsync(user);
        }

        // ================================
        // PASSWORD MANAGEMENT
        // ================================
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            // Generate reset link
            var Id = user.Id;
            var link = $"https://yourfrontend.com/reset-password?token={Id}";

            // 5. HTML Email Body
            var emailBody = $@"
<div style='font-family:Arial, sans-serif; max-width:600px; margin:auto; background:#f8f9fa; padding:30px; border-radius:10px;'>
    <div style='text-align:center; margin-bottom:20px;'>
        <img src='https://yourcdn.com/logo.png' alt='Skill Exchange' style='width:120px;'>
    </div>
    <h2 style='color:#2c3e50; text-align:center;'>Reset Your Password</h2>
    <p style='color:#555;'>Hello,</p>
    <p style='color:#555;'>We received a request to reset your password. Click the button below to continue:</p>
    <div style='text-align:center; margin:30px 0;'>
        <a href='{link}' style='background:#3498db; color:white; padding:14px 28px; text-decoration:none; font-weight:bold; border-radius:6px; display:inline-block;'>
            Reset Password
        </a>
    </div>
    <p style='color:#555;'>This link will expire in <strong>30 minutes</strong>.</p>
    <p style='color:#888; font-size:12px;'>If you didn’t request this, please ignore this email.</p>
    <hr style='border:none; border-top:1px solid #eee; margin:20px 0;'/>
    <p style='color:#888; font-size:12px; text-align:center;'>Thanks,<br/>The Skill Exchange Team</p>
</div>
";


            // 6. Send Email
            var isSent = await _emailService.SendEmailAsync(
                email,
                "Skill Exchange - Password Reset",
                emailBody,
                link // fallback plain link
            );

            return isSent;
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
            await _unitOfWork.Users.UpdateAsync(user);
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
        private string GenerateVerificationCode(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code = new char[length];
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];

            rng.GetBytes(bytes);
            for (int i = 0; i < length; i++)
            {
                code[i] = chars[bytes[i] % chars.Length];
            }

            return new string(code);
        }

    }
}
