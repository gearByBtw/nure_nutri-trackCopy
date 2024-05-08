using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NutritionalRecipeBook.Domain.Entities;
using System.Reflection;

namespace NutritionalRecipeBook.Infrastructure
{
    public class RecipeBookContext : IdentityDbContext<User>
    {
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients{ get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public DbSet<UserRecipe> UserRecipes { get; set; }

        public DbSet<UserIngredient> UserIngredients { get; set; }

        public DbSet<User> Users { get; set; }

        public RecipeBookContext(DbContextOptions<RecipeBookContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
