using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ae_resume_api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Access = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "SectorType",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorType", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    TemplateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Edited = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.TemplateId);
                });

            migrationBuilder.CreateTable(
                name: "Workspace",
                columns: table => new
                {
                    WorkspaceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proposal_Number = table.Column<int>(type: "int", nullable: false),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creation_Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspace", x => x.WorkspaceId);
                    table.ForeignKey(
                        name: "FK_Workspace_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateSector",
                columns: table => new
                {
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateSector", x => new { x.TemplateId, x.TypeId });
                    table.ForeignKey(
                        name: "FK_TemplateSector_SectorType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "SectorType",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateSector_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Template",
                        principalColumn: "TemplateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resume",
                columns: table => new
                {
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation_Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Edited = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<int>(type: "int", nullable: true),
                    TemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resume", x => x.ResumeId);
                    table.ForeignKey(
                        name: "FK_Resume_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resume_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Template",
                        principalColumn: "TemplateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resume_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "WorkspaceId");
                });

            migrationBuilder.CreateTable(
                name: "Sector",
                columns: table => new
                {
                    SectorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation_Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Edited = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.SectorId);
                    table.ForeignKey(
                        name: "FK_Sector_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "ResumeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sector_SectorType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "SectorType",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resume_EmployeeId",
                table: "Resume",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Resume_TemplateId",
                table: "Resume",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Resume_WorkspaceId",
                table: "Resume",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sector_ResumeId",
                table: "Sector",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sector_TypeId",
                table: "Sector",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SectorType_Title",
                table: "SectorType",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemplateSector_TypeId",
                table: "TemplateSector",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Workspace_EmployeeId",
                table: "Workspace",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sector");

            migrationBuilder.DropTable(
                name: "TemplateSector");

            migrationBuilder.DropTable(
                name: "Resume");

            migrationBuilder.DropTable(
                name: "SectorType");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropTable(
                name: "Workspace");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
