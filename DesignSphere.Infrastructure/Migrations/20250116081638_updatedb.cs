using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProjectContributers_ContactId",
                table: "ProjectContributers",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectContributers_Contacts_ContactId",
                table: "ProjectContributers",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectContributers_Contacts_ContactId",
                table: "ProjectContributers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectContributers_ContactId",
                table: "ProjectContributers");
        }
    }
}
