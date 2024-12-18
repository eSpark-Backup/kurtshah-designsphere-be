using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace designsphere_identity.STS.Identity.Migrations
{
    public partial class userdetailgooglekeycolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleTwoFactorSecretKey",
                table: "UserDetails",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleTwoFactorSecretKey",
                table: "UserDetails");
        }
    }
}
