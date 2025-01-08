using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectContributers_ProjectRoles_ProjectRoleId",
                table: "ProjectContributers");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProjectRoleId",
                table: "ProjectContributers",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

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

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProjectRoleId",
                table: "ProjectContributers",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

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
