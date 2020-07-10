using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesService.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingBasketItems",
                columns: table => new
                {
                    ShoppingBasketID = table.Column<Guid>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingBasketItems", x => new { x.ShoppingBasketID, x.ProductID });
                });

            migrationBuilder.CreateTable(
                name: "ShoppingBasketProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingBasketProducts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingBasketProducts_ProductId",
                table: "ShoppingBasketProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingBasketItems");

            migrationBuilder.DropTable(
                name: "ShoppingBasketProducts");
        }
    }
}
