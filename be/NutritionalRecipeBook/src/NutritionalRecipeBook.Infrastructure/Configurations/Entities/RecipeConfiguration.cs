using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NutritionalRecipeBook.Domain.Entities;

namespace NutritionalRecipeBook.Infrastructure.Configurations.Entities
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.Property(r => r.Name)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(r => r.Description)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.CookingProcess)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(r => r.UserId);

            builder.HasMany(r => r.Reviews)
                .WithOne()
                .HasForeignKey(r => r.RecipeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
