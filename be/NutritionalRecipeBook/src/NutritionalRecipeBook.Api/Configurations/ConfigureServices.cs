using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using NutritionalRecipeBook.Application.Common.Models;
using NutritionalRecipeBook.Application.Contracts;
using NutritionalRecipeBook.Application.Services;
using NutritionalRecipeBook.Domain.Entities;
using NutritionalRecipeBook.Infrastructure;
using NutritionalRecipeBook.Infrastructure.Contracts;
using NutritionalRecipeBook.Infrastructure.Repositories;
using NutritionalRecipeBook.Infrastructure.Services;
using Nutritionix;
using System.Reflection;

namespace NutritionalRecipeBook.Api.Configurations
{
    public static class ConfigureServices
    {
        private const string ConnetionStringKey = "RecipeBookDB";

        public static IServiceCollection AddServices(this WebApplicationBuilder builder, IConfiguration config)
        {
            var services = builder.Services;

            services.AddDbContext<DbContext, RecipeBookContext>(options => options.UseSqlServer(config.GetConnectionString(ConnetionStringKey)));

            services.AddHttpContextAccessor();

            services.AddAutoMapper(Assembly.Load("NutritionalRecipeBook.Application"));

            services.AddScoped<INutritionixClient, NutritionixClient>();

            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddScoped<IEmailSender<User>, EmailSender>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IRecipeRepository, RecipeRepository>();

            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<INutritionService, NutritionService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIdentityService, IdentityService>();

            services.Configure<EmailConfiguration>(config.GetSection("EmailConfiguration"));
            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
            services.Configure<NutritionixSettings>(config.GetSection("NutritionixSettings"));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());



            return services;
        }
    }
}