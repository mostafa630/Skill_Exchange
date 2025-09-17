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
        Task<RegisterResponseDto> RegisterAsync(RegisterRequserDto request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task LogoutAsync(Guid userId);

        // -------------------------
        // Google
        // -------------------------
        // Task<AuthResponseDTO> GoogleSignupAsync(GoogleSignupRequestDto request);
        // Task<AuthResponseDTO> GoogleLoginAsync(GoogleLoginRequestDto request);

    }
}
