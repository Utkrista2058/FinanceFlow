//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SmartBudgetTracker.DTOs;
//using SmartBudgetTracker.Models;
//using SmartBudgetTracker.Services; // Add this
//using FirebaseAdmin.Auth;

//namespace SmartBudgetTracker.Controllers.Api
//{
//    // Controllers/AuthController.cs
//    [Route("api/[controller]")]
//    [ApiController]

//    public class AuthController : ControllerBase
//    {
//        private readonly IAuthService _authService;

//        public AuthController(IAuthService authService)
//        {
//            _authService = authService;
//        }

//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
//        {
//            try
//            {
//                var response = await _authService.RegisterAsync(registerDto);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
//        {
//            try
//            {
//                var response = await _authService.LoginAsync(loginDto);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return Unauthorized(new { message = ex.Message });
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBudgetTracker.DTOs;
using SmartBudgetTracker.Models;
using SmartBudgetTracker.Services; // Add this
using FirebaseAdmin.Auth; // Add this

namespace SmartBudgetTracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var response = await _authService.RegisterAsync(registerDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // Add this Google Login endpoint
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto googleLoginDto)
        {
            try
            {
                // Verify the Firebase token
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(googleLoginDto.IdToken);

                string email = decodedToken.Claims["email"].ToString();
                string name = decodedToken.Claims.ContainsKey("name")
                    ? decodedToken.Claims["name"].ToString()
                    : email.Split('@')[0];
                string firebaseUid = decodedToken.Uid;

                // Use your auth service to handle Google login
                var response = await _authService.GoogleLoginAsync(email, name, firebaseUid);
                return Ok(response);
            }
            catch (FirebaseAuthException ex)
            {
                return Unauthorized(new { message = "Invalid Google token", error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
