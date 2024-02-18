using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cards.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial_create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CardOwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardItem_CardUser_CardOwnerId",
                        column: x => x.CardOwnerId,
                        principalTable: "CardUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardItem_CardUser_UserId",
                        column: x => x.UserId,
                        principalTable: "CardUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardItem_CardOwnerId",
                table: "CardItem",
                column: "CardOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CardItem_UserId",
                table: "CardItem",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardItem");

            migrationBuilder.DropTable(
                name: "CardUser");
        }
    }
}
