using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updateentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectContributers_ProjectRoles_ProjectRoleId",
                table: "ProjectContributers");

            migrationBuilder.DropColumn(
                name: "ProjectUserRole",
                table: "Contacts");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProjectRoleId",
                table: "ProjectContributers",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.AddColumn<int>(
                name: "ProjectUserRole",
                table: "ProjectContributers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProjectRoleId",
                table: "Contacts",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectContributers_ProjectRoles_ProjectRoleId",
                table: "ProjectContributers",
                column: "ProjectRoleId",
                principalTable: "ProjectRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectContributers_ProjectRoles_ProjectRoleId",
                table: "ProjectContributers");

            migrationBuilder.DropColumn(
                name: "ProjectUserRole",
                table: "ProjectContributers");

            migrationBuilder.DropColumn(
                name: "ProjectRoleId",
                table: "Contacts");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProjectRoleId",
                table: "ProjectContributers",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectUserRole",
                table: "Contacts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectContributers_ProjectRoles_ProjectRoleId",
                table: "ProjectContributers",
                column: "ProjectRoleId",
                principalTable: "ProjectRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
