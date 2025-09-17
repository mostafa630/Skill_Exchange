using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Skill_Exchange.Application.DTOs.Auth;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.Interfaces;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Skill_Exchange.Infrastructure.AuthenticationServices
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordHasher<AppUser> _passwordHasher = new();
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        public AuthService(IUnitOfWork unitOfWork,IJwtService jwtService,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _mapper = mapper;
        }
        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDto request)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user == null) 
                throw new Exception("Invalid email or password.");
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

        // Task<AuthResponseDTO> IAuthService.GoogleLoginAsync(GoogleLoginRequestDto request)
        // {
        //     throw new NotImplementedException();
        // }

        // Task<AuthResponseDTO> IAuthService.GoogleSignupAsync(GoogleSignupRequestDto request)
        // {
        //     throw new NotImplementedException();
        // }

        private async Task<LoginResponseDTO> GenerateLoginResponseAsync(AppUser user)
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

            return new LoginResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                Userinfo = _mapper.Map<UserDTO>(user)
            };


        }
    }
}