using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Skill_Exchange.Application.DTOs.Auth;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.Interfaces;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Application.DTOs.User;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.ObjectPool;

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

        public async Task<RegisterResponseDto> RegisterAsync(CreateUserDTO createRequestDTO)
        {

            // check if user with that email already exists
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(createRequestDTO.Email);
            if (existingUser != null)
            {
                return new RegisterResponseDto()
                {
                    Message = "User with that email already exists"
                };
            }
            // map from CreateUserDTO -----> user
            var user = _mapper.Map<AppUser>(createRequestDTO);
            user.PasswordHash = _passwordHasher.HashPassword(user, createRequestDTO.Password);
            _passwordHasher.HashPassword(user, createRequestDTO.Password);
            // save that user to the Db with EmailConfirmed = false
            var is_user_created = await _unitOfWork.Users.AddAsync(user);

            if (!is_user_created)
            {
                return new RegisterResponseDto()
                {
                    Message = "Regsiteration Failed"
                };
            }
            await _unitOfWork.CompleteAsync();

            var link = $"https://yourfrontend.com/verify-email?userId={user.Id}&token={null}";
            if (!string.IsNullOrEmpty(user.Email))
                await _emailService.SendEmailAsync(user.Email, link);

            return new RegisterResponseDto
            {
                Message = "Registeration done and you need to verify your email first"
            };
        }
        public async Task<bool> ConfirmEmailAsync(ConfirmEmailRequestDto request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("User not found.");

            user.EmailConfirmed = true;
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // Task<AuthResponseDTO> IAuthService.GoogleLoginAsync(GoogleLoginRequestDto request)
        // {
        //     throw new NotImplementedException();
        // }

        // Task<AuthResponseDTO> IAuthService.GoogleSignupAsync(GoogleSignupRequestDto request)
        // {
        //     throw new NotImplementedException();
        // }

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
    }
}