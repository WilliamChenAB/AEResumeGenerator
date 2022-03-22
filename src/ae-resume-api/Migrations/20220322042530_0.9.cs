using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ae_resume_api.Migrations
{
    public partial class _09 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EID",
                table: "Resume_Template");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SectorType",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Division",
                table: "Sector",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Sector",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SectorType_Title",
                table: "SectorType",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SectorType_Title",
                table: "SectorType");

            migrationBuilder.DropColumn(
                name: "Division",
                table: "Sector");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Sector");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SectorType",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "EID",
                table: "Resume_Template",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
