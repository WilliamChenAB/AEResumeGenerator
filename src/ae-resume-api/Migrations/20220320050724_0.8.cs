using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ae_resume_api.Migrations
{
    public partial class _08 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Last_Edited",
                table: "Resume_Template",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Last_Edited",
                table: "Resume_Template");
        }
    }
}
