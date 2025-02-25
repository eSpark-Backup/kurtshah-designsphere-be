using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace designsphere_identity.Admin.EntityFramework.PostgreSQL.Migrations.Identity
{
    public partial class MultiTenancy004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organization_OrganizationId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Organization",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "ConnectionString",
                table: "Organization",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "AdminUserId",
                table: "Organization",
                newName: "SupportEmailAddress");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Organization",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AuthConnectionString",
                table: "Organization",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Organization",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Organization",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Organization",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastmodificationTime",
                table: "Organization",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceConnectionString",
                table: "Organization",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubDomain",
                table: "Organization",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrganizationConfiguration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfigKey = table.Column<string>(type: "text", nullable: true),
                    ConfigValue = table.Column<string>(type: "text", nullable: true),
                    ConfigType = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationConfiguration_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationConfiguration_OrganizationId",
                table: "OrganizationConfiguration",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organization_OrganizationId",
                table: "Users",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organization_OrganizationId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "OrganizationConfiguration");

            migrationBuilder.DropColumn(
                name: "AuthConnectionString",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "LastmodificationTime",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "ResourceConnectionString",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "SubDomain",
                table: "Organization");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Organization",
                newName: "ConnectionString");

            migrationBuilder.RenameColumn(
                name: "SupportEmailAddress",
                table: "Organization",
                newName: "AdminUserId");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Organization",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationId",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Organization",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organization_OrganizationId",
                table: "Users",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id");
        }
    }
}
