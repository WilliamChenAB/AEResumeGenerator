using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ae_resume_api.Migrations
{
    public partial class nullableTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resume_Template_TemplateId",
                table: "Resume");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "Resume",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Resume_Template_TemplateId",
                table: "Resume",
                column: "TemplateId",
                principalTable: "Template",
                principalColumn: "TemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resume_Template_TemplateId",
                table: "Resume");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "Resume",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Resume_Template_TemplateId",
                table: "Resume",
                column: "TemplateId",
                principalTable: "Template",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
