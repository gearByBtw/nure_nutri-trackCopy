using WebApi.Models;

namespace WebApi.Services.Interfaces;

public interface IUserService
{
    Task<Result<bool>> AddUserAsync(AppUser user, string password);
    Task<Result<bool>> LoginAsync(AppUser user, string password);
    Task<Result<bool>> LogoutAsync();
    Task<Result<AppUser>> FindUserByEmail(string email);
}