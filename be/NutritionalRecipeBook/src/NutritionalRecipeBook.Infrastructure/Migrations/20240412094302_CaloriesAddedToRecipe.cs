using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutritionalRecipeBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CaloriesAddedToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Calories",
                table: "Recipes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calories",
                table: "Recipes");
        }
    }
}
