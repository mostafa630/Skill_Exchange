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
        Task<bool> StartRegisterAsync(string email);
        Task<bool> ConfirmEmailAsync(string verificationCode);
        Task<RegisterResponseDto> CompleteRegisterAsync(CreateUserDTO request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task LogoutAsync(Guid userId);

        

        // -------------------------
        // Google
        // -------------------------
        Task<RegisterResponseDto> GoogleSignupAsync(GoogleSignupRequestDto request);
        Task<LoginResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request);
        // -------------------------
        // Password Management
        // -------------------------
        /*Task ForgotPasswordAsync(ForgotPasswordRequestDto request);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request);


        // -------------------------
        // Refresh Tokens
        // -------------------------
        Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);*/
    }
}
