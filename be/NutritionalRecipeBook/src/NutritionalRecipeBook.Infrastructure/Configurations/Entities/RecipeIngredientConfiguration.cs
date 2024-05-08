using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Infrastructure.Configurations.Entities
{
    public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            builder.Ignore(ri => ri.Id);
        }
    }
}
