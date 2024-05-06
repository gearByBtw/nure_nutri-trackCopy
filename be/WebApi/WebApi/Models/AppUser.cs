using Microsoft.AspNetCore.Identity;

namespace WebApi.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}