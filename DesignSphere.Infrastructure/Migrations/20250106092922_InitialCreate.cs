using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Catalogs",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HardwareCategories",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HardwaresSeries",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwaresSeries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDetails",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    ComapanyId = table.Column<byte[]>(type: "bytea", nullable: false),
                    Unit = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ProjectId = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectRoles",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeviverableDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Stage = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    HelpUs = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: true),
                    OwnerName = table.Column<string>(type: "text", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserComnpanies",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserComnpanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contarctors",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    ContarctorName = table.Column<string>(type: "text", nullable: false),
                    AddressId = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contarctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contarctors_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoorFrames",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    MaterialId = table.Column<byte[]>(type: "bytea", nullable: false),
                    MaterialDescription = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    Thickness = table.Column<float>(type: "real", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    ManufacturerId = table.Column<byte[]>(type: "bytea", nullable: false),
                    ManufacturerName = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorFrames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoorFrames_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoorFrames_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doors",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    MaterialId = table.Column<byte[]>(type: "bytea", nullable: false),
                    MaterialDescription = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    Thickness = table.Column<float>(type: "real", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    ManufacturerId = table.Column<byte[]>(type: "bytea", nullable: false),
                    ManufacturerName = table.Column<string>(type: "text", nullable: false),
                    PanelType = table.Column<int>(type: "integer", nullable: false),
                    Hand = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doors_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doors_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PowerSupplies",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    MaterialId = table.Column<byte[]>(type: "bytea", nullable: false),
                    MaterialDescription = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    Thickness = table.Column<float>(type: "real", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    ManufacturerId = table.Column<byte[]>(type: "bytea", nullable: false),
                    ManufacturerName = table.Column<string>(type: "text", nullable: false),
                    Current = table.Column<float>(type: "real", nullable: false),
                    VoltageString = table.Column<float>(type: "real", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerSupplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerSupplies_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PowerSupplies_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectLabel",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    ColorText = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<byte[]>(type: "bytea", nullable: true),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectLabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectLabel_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectPlans",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    ProjectId = table.Column<byte[]>(type: "bytea", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPlans_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUserRoles",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    ProjectRoleId = table.Column<byte[]>(type: "bytea", nullable: false),
                    ProjectId = table.Column<byte[]>(type: "bytea", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectUserRoles_ProjectRoles_ProjectRoleId",
                        column: x => x.ProjectRoleId,
                        principalTable: "ProjectRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUserRoles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HardwareSets",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    MaterialId = table.Column<byte[]>(type: "bytea", nullable: false),
                    MaterialDescription = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    Thickness = table.Column<float>(type: "real", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    ManufacturerId = table.Column<byte[]>(type: "bytea", nullable: false),
                    ManufacturerName = table.Column<string>(type: "text", nullable: false),
                    HardwareCategoryId = table.Column<byte[]>(type: "bytea", nullable: false),
                    HardwareCategoryDescription = table.Column<string>(type: "text", nullable: false),
                    HardwareSeriesId = table.Column<byte[]>(type: "bytea", nullable: false),
                    HardwareSeriesDescription = table.Column<string>(type: "text", nullable: false),
                    PowerSupplyId = table.Column<byte[]>(type: "bytea", nullable: false),
                    PowerSupplyDescription = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HardwareSets_HardwareCategories_HardwareCategoryId",
                        column: x => x.HardwareCategoryId,
                        principalTable: "HardwareCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HardwareSets_HardwaresSeries_HardwareSeriesId",
                        column: x => x.HardwareSeriesId,
                        principalTable: "HardwaresSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HardwareSets_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HardwareSets_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HardwareSets_PowerSupplies_PowerSupplyId",
                        column: x => x.PowerSupplyId,
                        principalTable: "PowerSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reaction",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    PostId = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reaction_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoorOpenings",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Thickness = table.Column<float>(type: "real", nullable: false),
                    Mode = table.Column<int>(type: "integer", nullable: false),
                    ActiveStopCondition = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    DoorMaterial = table.Column<int>(type: "integer", nullable: false),
                    FrameMaterial = table.Column<int>(type: "integer", nullable: false),
                    DoorFrameId = table.Column<byte[]>(type: "bytea", nullable: true),
                    DoorFrameDescription = table.Column<string>(type: "text", nullable: true),
                    ProjectPlanId = table.Column<byte[]>(type: "bytea", nullable: false),
                    DoorId = table.Column<byte[]>(type: "bytea", nullable: true),
                    DoorDescription = table.Column<string>(type: "text", nullable: true),
                    HardwareSetId = table.Column<byte[]>(type: "bytea", nullable: true),
                    HardwareSetDescription = table.Column<string>(type: "text", nullable: true),
                    CreatorId = table.Column<string>(type: "text", nullable: true),
                    CreatorName = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastmodifierId = table.Column<string>(type: "text", nullable: true),
                    LastmodifierName = table.Column<string>(type: "text", nullable: true),
                    LastmodificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorOpenings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoorOpenings_DoorFrames_DoorFrameId",
                        column: x => x.DoorFrameId,
                        principalTable: "DoorFrames",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoorOpenings_Doors_DoorId",
                        column: x => x.DoorId,
                        principalTable: "Doors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoorOpenings_HardwareSets_HardwareSetId",
                        column: x => x.HardwareSetId,
                        principalTable: "HardwareSets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoorOpenings_ProjectPlans_ProjectPlanId",
                        column: x => x.ProjectPlanId,
                        principalTable: "ProjectPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contarctors_AddressId",
                table: "Contarctors",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorFrames_ManufacturerId",
                table: "DoorFrames",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorFrames_MaterialId",
                table: "DoorFrames",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorOpenings_DoorFrameId",
                table: "DoorOpenings",
                column: "DoorFrameId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorOpenings_DoorId",
                table: "DoorOpenings",
                column: "DoorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorOpenings_HardwareSetId",
                table: "DoorOpenings",
                column: "HardwareSetId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorOpenings_ProjectPlanId",
                table: "DoorOpenings",
                column: "ProjectPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_ManufacturerId",
                table: "Doors",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_MaterialId",
                table: "Doors",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareSets_HardwareCategoryId",
                table: "HardwareSets",
                column: "HardwareCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareSets_HardwareSeriesId",
                table: "HardwareSets",
                column: "HardwareSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareSets_ManufacturerId",
                table: "HardwareSets",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareSets_MaterialId",
                table: "HardwareSets",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareSets_PowerSupplyId",
                table: "HardwareSets",
                column: "PowerSupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ProjectId",
                table: "Post",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerSupplies_ManufacturerId",
                table: "PowerSupplies",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerSupplies_MaterialId",
                table: "PowerSupplies",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLabel_ProjectId",
                table: "ProjectLabel",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPlans_ProjectId",
                table: "ProjectPlans",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRoles_ProjectId",
                table: "ProjectUserRoles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUserRoles_ProjectRoleId",
                table: "ProjectUserRoles",
                column: "ProjectRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Reaction_PostId",
                table: "Reaction",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catalogs");

            migrationBuilder.DropTable(
                name: "Contarctors");

            migrationBuilder.DropTable(
                name: "DoorOpenings");

            migrationBuilder.DropTable(
                name: "ProjectDetails");

            migrationBuilder.DropTable(
                name: "ProjectLabel");

            migrationBuilder.DropTable(
                name: "ProjectUserRoles");

            migrationBuilder.DropTable(
                name: "Reaction");

            migrationBuilder.DropTable(
                name: "UserComnpanies");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "DoorFrames");

            migrationBuilder.DropTable(
                name: "Doors");

            migrationBuilder.DropTable(
                name: "HardwareSets");

            migrationBuilder.DropTable(
                name: "ProjectPlans");

            migrationBuilder.DropTable(
                name: "ProjectRoles");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "HardwareCategories");

            migrationBuilder.DropTable(
                name: "HardwaresSeries");

            migrationBuilder.DropTable(
                name: "PowerSupplies");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "Materials");
        }
    }
}
