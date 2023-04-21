using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KcalCalc.Data.Migrations
{
    public partial class PersonUserForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserID",
                table: "Persons",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_IdentityUserID",
                table: "Persons",
                column: "IdentityUserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_AspNetUsers_IdentityUserID",
                table: "Persons",
                column: "IdentityUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_AspNetUsers_IdentityUserID",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_IdentityUserID",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "IdentityUserID",
                table: "Persons");
        }
    }
}
