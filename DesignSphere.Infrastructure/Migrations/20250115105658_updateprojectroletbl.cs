using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateprojectroletbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectUserRole",
                table: "ProjectContributers");

            migrationBuilder.AddColumn<bool>(
                name: "IsContributerRemoved",
                table: "ProjectContributers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsContributerRemoved",
                table: "ProjectContributers");

            migrationBuilder.AddColumn<int>(
                name: "ProjectUserRole",
                table: "ProjectContributers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
