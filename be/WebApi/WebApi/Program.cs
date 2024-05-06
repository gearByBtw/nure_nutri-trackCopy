using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Data.Interfaces;
using WebApi.Models;
using WebApi.Services;
using WebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<WebApi.Data.AppContext>(options =>
{
    options.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddDefaultTokenProviders().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WebApi.Data.AppContext>()
    .AddSignInManager<SignInManager<AppUser>>();

builder.Services.AddScoped<UserManager<AppUser>>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();