using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace designsphere_identity.STS.Identity.Migrations
{
    public partial class identityuserupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ExUserIdentities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ExUserIdentities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NewsLetterSubscription",
                table: "ExUserIdentities",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "ExUserIdentities",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRole", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "JobRole");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ExUserIdentities");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ExUserIdentities");

            migrationBuilder.DropColumn(
                name: "NewsLetterSubscription",
                table: "ExUserIdentities");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "ExUserIdentities");
        }
    }
}
