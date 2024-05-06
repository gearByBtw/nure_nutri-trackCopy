using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data;

public class AppContext : IdentityDbContext<AppUser>
{
    public AppContext(DbContextOptions<AppContext> options) : base(options)
    {
    }
}

//dotnet ef migrations add InitialCreating --project "C:\Users\khanina.d\Desktop\nure_nutri-track\be\WebApi\WebApi\WebApi.csproj" (your path)
//dotnet ef database update  --project "C:\Users\khanina.d\Desktop\nure_nutri-track\be\WebApi\WebApi\WebApi.csproj" (your path)
