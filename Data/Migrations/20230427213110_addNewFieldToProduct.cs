using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KcalCalc.Data.Migrations
{
    public partial class addNewFieldToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Kcal",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kcal",
                table: "Products");
        }
    }
}
