using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickBite.Migrations
{
    /// <inheritdoc />
    public partial class customerMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Customer",
                columns: table => new
                {
                    Customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Customer", x => x.Customer_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Customer");
        }
    }
}
