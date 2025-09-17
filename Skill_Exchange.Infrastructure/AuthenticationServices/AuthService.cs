using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Skill_Exchange.Application.DTOs.Auth;
using Skill_Exchange.Application.Interfaces;

namespace Skill_Exchange.Infrastructure.AuthenticationServices
{
    public class AuthService : IAuthService
    {
        private readonly JwtOptions _jwtOptions;
        public AuthService(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }
        public Task<LoginResponseDTO> LoginAsync(LoginRequestDto request)
        {
            throw new NotImplementedException();
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
    }
}