using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriConnect_MVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class approveField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Approved",
                table: "Farmers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Farmers");
        }
    }
}
