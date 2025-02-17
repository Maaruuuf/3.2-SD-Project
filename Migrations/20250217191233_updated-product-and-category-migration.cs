using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickBite.Migrations
{
    /// <inheritdoc />
    public partial class updatedproductandcategorymigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_Product_cat_id",
                table: "tbl_Product",
                column: "cat_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Product_tbl_Category_cat_id",
                table: "tbl_Product",
                column: "cat_id",
                principalTable: "tbl_Category",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Product_tbl_Category_cat_id",
                table: "tbl_Product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Product_cat_id",
                table: "tbl_Product");
        }
    }
}
