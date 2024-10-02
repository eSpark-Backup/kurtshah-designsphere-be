using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace designsphere_identity.STS.Identity.Migrations;

public partial class userdetaillog : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UserDetailLogs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserIdentityId = table.Column<string>(type: "text", nullable: true),
                UserName = table.Column<string>(type: "text", nullable: true),
                OTPRetries = table.Column<byte>(type: "smallint", nullable: false),
                OTPStatus = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "text", nullable: true),
                AppHash = table.Column<string>(type: "text", nullable: true),
                NotificationId = table.Column<string>(type: "text", nullable: true),
                DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                CreatorId = table.Column<string>(type: "text", nullable: true),
                LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserDetailLogs", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserDetailLogs");
    }
}
