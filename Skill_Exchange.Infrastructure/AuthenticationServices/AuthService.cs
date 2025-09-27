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
        public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user == null)
                return Result<LoginResponseDto>.Fail("Invalid email or password.");

            if (!user.EmailConfirmed)
                return Result<LoginResponseDto>.Fail("This Email is not Confirmed.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return Result<LoginResponseDto>.Fail("Invalid email or password.");

            _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            var loginResponse = await GenerateLoginResponseAsync(user);
            return Result<LoginResponseDto>.Ok(loginResponse);
        }


        public Task LogoutAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterResponseDto> CompleteRegisterAsync(CreateUserDTO createRequestDTO)
        {

            // check if user with that email already exists
            var PendingVerification = await _unitOfWork.PendingVerifications.GetByEmailAsync(createRequestDTO.Email);
            if (PendingVerification == null)
            {
                return new RegisterResponseDto()
                {
                    Message = "You Must Register First"
                };
            }
            if (!PendingVerification.IsConfirmed)
            {
                return new RegisterResponseDto()
                {
                    Message = "Mail is not Confirmed"
                };

            }

            // map from CreateUserDTO -----> user
            var user = _mapper.Map<AppUser>(createRequestDTO);
            user.PasswordHash = _passwordHasher.HashPassword(user, createRequestDTO.Password);
            user.EmailConfirmed = true;
            // save that user to the Db with EmailConfirmed = false
            var is_user_created = await _unitOfWork.Users.AddAsync(user);

            if (!is_user_created)
            {
                return new RegisterResponseDto()
                {
                    Message = "Regsiteration Failed!"
                };
            }
            await _unitOfWork.PendingVerifications.DeleteAsync(PendingVerification);
            await _unitOfWork.CompleteAsync();

            return new RegisterResponseDto
            {
                Message = "Registeration succeded!"
            };
        }

        private async Task<LoginResponseDto> GenerateLoginResponseAsync(AppUser user)
        {
            var claims = new[]
            {
                new Claim(System.Security.Claims.ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(System.Security.Claims.ClaimTypes.Email,user.Email),
                new Claim(System.Security.Claims.ClaimTypes.Name,$"{user.FirstName} {user.LastName}")
            };
            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken();

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

        public async Task<Result<bool>> StartRegisterAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user != null)
                return Result<bool>.Fail("User already exists");

            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            string verificationCode = Convert.ToBase64String(randomBytes);

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
                var pendingVerification = await _unitOfWork.PendingVerifications
                .GetByVerificationCodeAsync(verificationCode);
                if (pendingVerification == null || pendingVerification.Expiry < DateTime.UtcNow)
                    return false;
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

        public async Task<RegisterResponseDto> GoogleSignupAsync(GoogleSignupRequestDto request)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user != null)
            {
                return new RegisterResponseDto()
                {
                    Message = "User with this email already exists."
                };
            }
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

            var newUser = new AppUser
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
            };

            newUser.EmailConfirmed = true;
            var is_user_created = await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.CompleteAsync();
            if (!is_user_created)
            {
                return new RegisterResponseDto()
                {
                    Message = "Regsiteration Failed!"
                };
            }
            return new RegisterResponseDto()
            {
                Message = "Registeration succeded!"
            };
        }

        public async Task<LoginResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            var user = await _unitOfWork.Users.GetByEmailAsync(payload.Email);
            if (user == null)
                throw new Exception("User not found. Please signup with Google first.");
            return await GenerateLoginResponseAsync(user);
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            var Id = user.Id;
            var link = $"https://yourfrontend.com/reset-password?token={Id}";

            var is_sent = await _emailService.SendEmailAsync(email, "Reset Password", "click here : ", link);

            if (!is_sent)
                return false;
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(request.Token);  // token here is user  id
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

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.OldPassword);
            if (result == PasswordVerificationResult.Failed)
                return Result<bool>.Fail("Current password is invalid");

            if (string.IsNullOrEmpty(request.NewPassword))
                return Result<bool>.Fail("New password is invalid");

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return Result<bool>.Ok(true);
        }


        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null) throw new Exception("Invalid token");

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _unitOfWork.Users.GetByIdAsync(Guid.Parse(userId!));
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

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

        private string GenerateToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}