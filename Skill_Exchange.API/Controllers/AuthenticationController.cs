using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost("start_register")]
        public async Task<IActionResult> StartRegister(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Email is required." });
            try
            {
                var result = await _authService.StartRegisterAsync(email);
                if (result)
                    return Ok(new { message = "Verification code sent to email." });
                else
                    return BadRequest(new { message = "Failed to send verification code." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("confirm_email")]
        public async Task<IActionResult> ConfirmEmail(string verificationCode)
        {
            if (string.IsNullOrEmpty(verificationCode))
                return BadRequest(new { message = "Verification code is required." });
            try
            {
                var result = await _authService.ConfirmEmailAsync(verificationCode);
                if (result)
                    return Ok(new { message = "Email confirmed successfully." });
                else
                    return BadRequest(new { message = "Invalid verification code." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("complete_register")]
        public async Task<IActionResult> CompleteRegister([FromBody] CreateUserDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.CompleteRegisterAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("google_login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.IdToken))
                return BadRequest(new { message = "IdToken is required." });

            try
            {
                var response = await _authService.GoogleLoginAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("google_signup")]
        public async Task<IActionResult> GoogleSignup([FromBody] GoogleSignupRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.IdToken))
                return BadRequest(new { message = "IdToken is required." });

            try
            {
                var response = await _authService.GoogleSignupAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("forget_password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (String.IsNullOrEmpty(email))
                return BadRequest(new { message = "Invalid Email" });
            try
            {
                var response = await _authService.ForgotPasswordAsync(email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("reset_password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            try
            {
                var response = await _authService.ResetPasswordAsync(resetPasswordRequestDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("change_password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto changePasswordRequestDto)
        {
            try
            {
                var response = await _authService.ChangePasswordAsync(changePasswordRequestDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refresh_token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(refreshTokenRequestDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
