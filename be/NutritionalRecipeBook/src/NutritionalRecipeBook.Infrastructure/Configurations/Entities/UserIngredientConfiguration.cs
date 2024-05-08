using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Infrastructure.Configurations.Entities
{
    public class UserIngredientConfiguration : IEntityTypeConfiguration<UserIngredient>
    {
        public void Configure(EntityTypeBuilder<UserIngredient> builder)
        {
            builder.HasKey(ri => new { ri.UserId, ri.IngredientId });
        }
    }
}
