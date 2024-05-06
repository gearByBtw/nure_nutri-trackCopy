using Microsoft.AspNetCore.Identity;
using WebApi.Enums;

namespace WebApi.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserRole Role { get; set; }
    public virtual List<Ingredient> BannedIngredients { get; set; }
    public string Subscription { get; set; }
}