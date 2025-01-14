using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addprojectcontributertbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ContributerId",
                table: "TaskAssignee",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateTable(
                name: "ProjectRoles",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectContributers",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    ContactId = table.Column<byte[]>(type: "bytea", nullable: false),
                    ProjectRoleId = table.Column<byte[]>(type: "bytea", nullable: false),
                    ProjectId = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectContributers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectContributers_ProjectRoles_ProjectRoleId",
                        column: x => x.ProjectRoleId,
                        principalTable: "ProjectRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectContributers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectContributers_ProjectId",
                table: "ProjectContributers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectContributers_ProjectRoleId",
                table: "ProjectContributers",
                column: "ProjectRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectContributers");

            migrationBuilder.DropTable(
                name: "ProjectRoles");

            migrationBuilder.DropColumn(
                name: "ContributerId",
                table: "TaskAssignee");
        }
    }
}
