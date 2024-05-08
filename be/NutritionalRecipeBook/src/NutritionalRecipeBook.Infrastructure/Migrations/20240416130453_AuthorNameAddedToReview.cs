using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutritionalRecipeBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuthorNameAddedToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Reviews",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Reviews");
        }
    }
}
