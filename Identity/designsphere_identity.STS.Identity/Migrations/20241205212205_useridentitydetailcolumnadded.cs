using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace designsphere_identity.STS.Identity.Migrations
{
    public partial class useridentitydetailcolumnadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "UserDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "UserDetails",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivacyPolicyAccepted",
                table: "UserDetails",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "JobRoleId",
                table: "UserDetails",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "UserDetails",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "IsPrivacyPolicyAccepted",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "JobRoleId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "UserDetails");
        }
    }
}
