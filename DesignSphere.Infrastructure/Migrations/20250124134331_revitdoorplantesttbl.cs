using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DesignSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class revitdoorplantesttbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestDoorPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectName = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    DoorId = table.Column<string>(type: "text", nullable: false),
                    DoorType = table.Column<string>(type: "text", nullable: false),
                    FloorName = table.Column<string>(type: "text", nullable: false),
                    DoorWidth = table.Column<string>(type: "text", nullable: false),
                    DoorHeight = table.Column<string>(type: "text", nullable: false),
                    LocationX = table.Column<double>(type: "double precision", nullable: false),
                    LocationY = table.Column<double>(type: "double precision", nullable: false),
                    LocationZ = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestDoorPlans", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestDoorPlans");
        }
    }
}
