using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutritionalRecipeBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ServiceSizeColumnNameChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServingSize",
                table: "Recipes",
                newName: "ServingSizeInGrams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServingSizeInGrams",
                table: "Recipes",
                newName: "ServingSize");
        }
    }
}
