using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class CaloriesAndRecepiesInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subscription",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Recepies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calories = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Votes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recepies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalorieNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Calorie = table.Column<int>(type: "int", nullable: false),
                    RecepieId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecepieName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecepieId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalorieNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalorieNotes_Recepies_RecepieId1",
                        column: x => x.RecepieId1,
                        principalTable: "Recepies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecepieId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredient_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ingredient_Recepies_RecepieId",
                        column: x => x.RecepieId,
                        principalTable: "Recepies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalorieNotes_RecepieId1",
                table: "CalorieNotes",
                column: "RecepieId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_AppUserId",
                table: "Ingredient",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_RecepieId",
                table: "Ingredient",
                column: "RecepieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalorieNotes");

            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "Recepies");

            migrationBuilder.DropColumn(
                name: "Subscription",
                table: "AspNetUsers");
        }
    }
}
