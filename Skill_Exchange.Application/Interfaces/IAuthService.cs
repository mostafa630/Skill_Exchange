using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Auth;
using Skill_Exchange.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Interfaces
{
    public interface IAuthService
    {
        // -------------------------
        // Register & Login
        // -------------------------
        Task<Result<bool>> StartRegisterAsync(string emaiL);
        Task<Result<bool>> ConfirmEmailAsync(ConfirmEmailRequestDto confirmEmailRequestDto);
        Task<RegisterResponseDto> CompleteRegisterAsync(CreateUserDTO request);
        Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request);
        Task LogoutAsync(Guid userId);



        // -------------------------
        // Google
        // -------------------------
        Task<RegisterResponseDto> GoogleSignupAsync(GoogleSignupRequestDto request);
        Task<LoginResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request);
        // -------------------------
        // Password Management
        // -------------------------
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request);
        Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequestDto request);

        // -------------------------
        // Refresh Tokens
        // -------------------------
        Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}
