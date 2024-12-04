using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace designsphere_identity.Admin.EntityFramework.PostgreSQL.Migrations.Identity
{
    public partial class DbInit4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivacyPolicyAccepted",
                table: "Users",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JobRoleId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsPrivacyPolicyAccepted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "JobRoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Users");
        }
    }
}
