using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Infrastructure.Configurations.Entities
{
    public class UserRecipeConfiguration : IEntityTypeConfiguration<UserRecipe>
    {
        public void Configure(EntityTypeBuilder<UserRecipe> builder)
        {
            builder.HasKey(ri => new { ri.RecipeId, ri.UserId });

            builder.HasOne(r => r.Recipe)
                .WithMany()
                .HasForeignKey(r => r.RecipeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
