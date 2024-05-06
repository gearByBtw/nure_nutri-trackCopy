using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Enums;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(RegisterDto model)
    {
        var appUser = new AppUser()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.FirstName + model.LastName,
            Role = UserRole.User
        };

        var createResult = await _userService.AddUserAsync(appUser, model.Password);

        if (!createResult.IsSuccessful)
        {
            return BadRequest(createResult.Message);
        }

        return Ok(appUser);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LogIn(LoginDto model)
    {
        var findAppUserResult = await _userService.FindUserByEmail(model.Email);

        if (!findAppUserResult.IsSuccessful)
        {
            return BadRequest(findAppUserResult.Message);
        }

        var loginResult = await _userService.LoginAsync(findAppUserResult.Data, model.Password);
            
        if (!loginResult.IsSuccessful)
        {
            return BadRequest(loginResult.Message);
        }

        return Ok(loginResult);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogOut()
    {
        var logoutResult = await _userService.LogoutAsync();

        if (!logoutResult.IsSuccessful)
        {
            return BadRequest(logoutResult.Message);
        }

        return Ok();
    }
    
    [Authorize]
    [HttpGet("l")]
    public async Task<IActionResult> LogOut1()
    {
        return Ok(22);
    }
}