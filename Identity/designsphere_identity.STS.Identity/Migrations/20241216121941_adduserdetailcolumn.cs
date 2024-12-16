using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace designsphere_identity.STS.Identity.Migrations
{
    public partial class adduserdetailcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsServiceAggrementAccepted",
                table: "UserDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsServiceAggrementAccepted",
                table: "UserDetails");
        }
    }
}
