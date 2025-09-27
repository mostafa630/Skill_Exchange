using Microsoft.AspNetCore.Mvc;
using Skill_Exchange.Application.DTOs.Auth;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.Interfaces;

namespace Skill_Exchange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        //  Step 1: Start Register (send verification email)
        [HttpPost("start_register")]
        public async Task<IActionResult> StartRegister([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email is required." });

            var result = await _authService.StartRegisterAsync(email);
            return result.Success
                ? Ok(new { message = "Verification code sent to email." })
                : BadRequest(new { message = result.Error });
        }

        //  Step 2: Confirm Email with verification code
        [HttpPost("confirm_email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] string verificationCode)
        {
            if (string.IsNullOrEmpty(verificationCode))
                return BadRequest(new { message = "Verification code is required." });

            var result = await _authService.ConfirmEmailAsync(verificationCode);
            return result
                ? Ok(new { message = "Email confirmed successfully." })
                : BadRequest(new { message = "Invalid or expired verification code." });
        }

        //  Step 3: Complete Register (create account)
        [HttpPost("complete_register")]
        public async Task<IActionResult> CompleteRegister([FromBody] CreateUserDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.CompleteRegisterAsync(request);
            return Ok(response);
        }

        //  Login with Email & Password
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(request);
            return result.Success
                ? Ok(result.Data)
                : Unauthorized(new { message = result.Error });
        }

        //  Google Login
        [HttpPost("google_login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.IdToken))
                return BadRequest(new { message = "IdToken is required." });

            var response = await _authService.GoogleLoginAsync(request);
            return Ok(response);
        }

        //  Google Signup
        [HttpPost("google_signup")]
        public async Task<IActionResult> GoogleSignup([FromBody] GoogleSignupRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.IdToken))
                return BadRequest(new { message = "IdToken is required." });

            var response = await _authService.GoogleSignupAsync(request);
            return Ok(response);
        }

        //  Forgot Password (send reset link)
        [HttpPost("forgot_password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Invalid Email" });

            var response = await _authService.ForgotPasswordAsync(email);
            return response
                ? Ok(new { message = "Password reset link sent to email." })
                : BadRequest(new { message = "Failed to send reset link." });
        }

        //  Reset Password
        [HttpPost("reset_password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var response = await _authService.ResetPasswordAsync(request);
            return response
                ? Ok(new { message = "Password reset successfully." })
                : BadRequest(new { message = "Reset password failed." });
        }

        //  Change Password
        [HttpPost("change_password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            var result = await _authService.ChangePasswordAsync(request);
            return result.Success
                ? Ok(new { message = "Password changed successfully." })
                : BadRequest(new { message = result.Error});
        }

        //  Refresh Token
        [HttpPost("refresh_token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var response = await _authService.RefreshTokenAsync(request);
            return Ok(response);
        }

        
        /*[HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] Guid userId)
        {
            await _authService.LogoutAsync(userId);
            return Ok(new { message = "User logged out successfully." });
        }*/
    }
}
