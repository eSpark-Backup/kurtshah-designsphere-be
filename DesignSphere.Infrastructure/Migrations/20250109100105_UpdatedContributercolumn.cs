using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedContributercolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectUserRole",
                table: "ProjectContributers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectUserRole",
                table: "ProjectContributers");
        }
    }
}
