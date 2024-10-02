using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace designsphere_identity.STS.Identity.Migrations;

public partial class Initial1 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Devices",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                DeviceCode = table.Column<string>(type: "text", nullable: true),
                DeviceStatus = table.Column<int>(type: "integer", nullable: false),
                CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                CreatorId = table.Column<string>(type: "text", nullable: true),
                LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Devices", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "UserDetails",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserIdentityId = table.Column<string>(type: "text", nullable: true),
                UserName = table.Column<string>(type: "text", nullable: true),
                NotificationId = table.Column<string>(type: "text", nullable: true),
                AppHash = table.Column<string>(type: "text", nullable: true),
                OTPRetries = table.Column<byte>(type: "smallint", nullable: false),
                OTPStatus = table.Column<int>(type: "integer", nullable: false),
                DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                IsSocialRegisteredWithPhonenumber = table.Column<bool>(type: "boolean", nullable: true),
                Name = table.Column<string>(type: "text", nullable: true),
                CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                CreatorId = table.Column<string>(type: "text", nullable: true),
                LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserDetails", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserDetails_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_UserDetails_DeviceId",
            table: "UserDetails",
            column: "DeviceId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserDetails");

        migrationBuilder.DropTable(
            name: "Devices");
    }
}
