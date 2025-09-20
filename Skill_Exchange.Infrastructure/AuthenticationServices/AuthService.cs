using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.ObjectPool;
using Skill_Exchange.Application.DTOs.Auth;
using Skill_Exchange.Application.DTOs.User;
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
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Invalid email or password.");
            if (!user.EmailConfirmed)
                throw new Exception("This Email is not Confirmed.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid email or password.");

            _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
            return await GenerateLoginResponseAsync(user);
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
        /*public async Task<bool> ConfirmEmailAsync(ConfirmEmailRequestDto request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("User not found.");

            user.EmailConfirmed = true;
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
            return true;
        }
*/
        
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

        public async Task<bool> StartRegisterAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user != null)
                throw new Exception("User already exists");


            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            string verificationCode = Convert.ToBase64String(randomBytes);
            var pending = await _unitOfWork.PendingVerifications.GetByEmailAsync(email);
            if(pending != null)
            {
                pending.VerificationCode= verificationCode;
                pending.Expiry= DateTime.UtcNow.AddMinutes(30);
                await _unitOfWork.PendingVerifications.UpdateAsync(pending);
                await _unitOfWork.CompleteAsync();
                var sent = await _emailService.SendEmailAsync(email, verificationCode);
                if (!sent)
                    return false;
                return true;
            }
            var isSent = await _emailService.SendEmailAsync(email, verificationCode);

            if(!isSent)
                return false;
            var pendingVerification = new PendingVerification
            {
                Email = email,
                VerificationCode = verificationCode,
                Expiry = DateTime.UtcNow.AddMinutes(30),
                IsConfirmed = false
            };
            await _unitOfWork.PendingVerifications.AddAsync(pendingVerification);
            await _unitOfWork.CompleteAsync();
            return true;
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


    }
}