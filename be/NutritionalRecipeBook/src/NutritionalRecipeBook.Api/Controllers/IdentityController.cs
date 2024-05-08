using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Application.Common.Models.User;
using NutritionalRecipeBook.Application.Contracts;
using NutritionalRecipeBook.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace NutritionalRecipeBook.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly IEmailSender<User> _emailSender;

        private readonly SignInManager<User> _signInManager;

        private readonly IIdentityService _identityService;

        private readonly JwtSettings _jwtSettings;

        public IdentityController(UserManager<User> userManager,
            IEmailSender<User> emailSender,
            SignInManager<User> signInManager,
            IOptionsSnapshot<JwtSettings> jwtSettings,
            IIdentityService identityService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _identityService = identityService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest model)
        {
            var result = await _identityService.Register(model);

            return result.Succeeded ? Ok("Registration successful. Please check your email for confirmation link.") : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _identityService.Login(request);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _identityService.ConfirmEmail(userId, token);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return result.Value.Succeeded ? Ok() : BadRequest(result.Value.Errors);
        }
    }
}
