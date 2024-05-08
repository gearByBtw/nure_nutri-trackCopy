using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Application.Common.Models.User;
using NutritionalRecipeBook.Application.Contracts;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Domain.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NutritionalRecipeBook.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;

        private readonly IEmailSender<User> _emailSender;

        private readonly SignInManager<User> _signInManager;

        private readonly JwtSettings _jwtSettings;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUrlHelperFactory _urlHelperFactory;

        public IdentityService(UserManager<User> userManager,
            IEmailSender<User> emailSender,
            SignInManager<User> signInManager,
            IOptionsSnapshot<JwtSettings> jwtSettings,
            IUrlHelperFactory urlHelperFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> Register(RegisterUserRequest request)
        {
            var user = new User { UserName = request.UserName, Email = request.Email };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await SendConfirmationEmailAsync(user, request.Email);
            }

            return result;
        }

        public async Task<Result<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Result<LoginResponse>.Failure(new Error("400", "Invalid email"));
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

                return Result<LoginResponse>.Success(new LoginResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    UserId = user.Id,
                });
            }

            return Result<LoginResponse>.Failure(new Error("400", "Invalid password"));
        }

        public async Task<User?> GetUserByIdWithRelationsAsync(string id)
        {
            return await _userManager.Users
                .Include(u => u.ShoppingList)
                .ThenInclude(u => u.Ingredient)
                .Include(u => u.FavoriteRecipes)
                .ThenInclude(ur => ur.Recipe)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> FindUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<Result<IdentityResult>> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Result<IdentityResult>.Failure(new Error("404", "Such user doesn't exist."));
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return Result<IdentityResult>.Success(result);
        }

        private async Task SendConfirmationEmailAsync(User user, string email)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = GenerateConfirmationLink(user.Id, token, _httpContextAccessor.HttpContext);
            await _emailSender.SendConfirmationLinkAsync(user, email, confirmationLink);
        }

        private string GenerateConfirmationLink(string userId, string token, HttpContext httpContext)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(httpContext, new RouteData(), new ActionDescriptor()));
            return urlHelper.Action("ConfirmEmail", "Identity", new { userId, token }, "https");
        }
    }
}
