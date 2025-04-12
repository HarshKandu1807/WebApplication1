using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class order3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProdudtIDProductId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProdudtIDProductId",
                table: "Orders",
                column: "ProdudtIDProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProdudtIDProductId",
                table: "Orders",
                column: "ProdudtIDProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProdudtIDProductId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProdudtIDProductId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProdudtIDProductId",
                table: "Orders");
        }
    }
}
