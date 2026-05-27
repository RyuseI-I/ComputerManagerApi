using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComputerManagerApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentManufacturers",
                columns: table => new
                {
                    Id             = table.Column<int>(type: "int", nullable: false)
                                         .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation   = table.Column<string>(type: "nvarchar(30)",  maxLength: 30,  nullable: false),
                    FullName       = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FoundationDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentManufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComponentTypes",
                columns: table => new
                {
                    Id           = table.Column<int>(type: "int", nullable: false)
                                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(30)",  maxLength: 30,  nullable: false),
                    Name         = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PCs",
                columns: table => new
                {
                    Id        = table.Column<int>(type: "int", nullable: false)
                                     .Annotation("SqlServer:Identity", "1, 1"),
                    Name      = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Weight    = table.Column<double>(type: "float", nullable: false),
                    Warranty  = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Stock     = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Code                     = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false),
                    Name                     = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description              = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComponentManufacturersId = table.Column<int>(type: "int", nullable: false),
                    ComponentTypesId         = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Components_ComponentManufacturers_ComponentManufacturersId",
                        column: x => x.ComponentManufacturersId,
                        principalTable: "ComponentManufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Components_ComponentTypes_ComponentTypesId",
                        column: x => x.ComponentTypesId,
                        principalTable: "ComponentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PCComponents",
                columns: table => new
                {
                    PcId          = table.Column<int>(type: "int", nullable: false),
                    ComponentCode = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false),
                    Amount        = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCComponents", x => new { x.PcId, x.ComponentCode });
                    table.ForeignKey(
                        name: "FK_PCComponents_Components_ComponentCode",
                        column: x => x.ComponentCode,
                        principalTable: "Components",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PCComponents_PCs_PcId",
                        column: x => x.PcId,
                        principalTable: "PCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // ── Seed: ComponentManufacturers ──────────────────────────────────
            migrationBuilder.InsertData(
                table: "ComponentManufacturers",
                columns: new[] { "Id", "Abbreviation", "FullName", "FoundationDate" },
                values: new object[,]
                {
                    { 1, "AMD",  "Advanced Micro Devices",  new DateOnly(1969, 5, 1)  },
                    { 2, "NV",   "NVIDIA Corporation",       new DateOnly(1993, 4, 5)  },
                    { 3, "COR",  "Corsair Gaming Inc.",      new DateOnly(1994, 1, 1)  },
                    { 4, "ASUS", "ASUSTeK Computer Inc.",   new DateOnly(1989, 4, 2)  }
                });

            // ── Seed: ComponentTypes ──────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "ComponentTypes",
                columns: new[] { "Id", "Abbreviation", "Name" },
                values: new object[,]
                {
                    { 1, "CPU", "Processor"         },
                    { 2, "GPU", "Graphics Card"     },
                    { 3, "RAM", "Memory"            },
                    { 4, "SSD", "Solid State Drive" }
                });

            // ── Seed: Components ──────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "Components",
                columns: new[] { "Code", "Name", "Description", "ComponentManufacturersId", "ComponentTypesId" },
                values: new object[,]
                {
                    { "CPU0000001", "Ryzen 7 7800X3D",              "8-core gaming processor with 3D V-Cache technology",      1, 1 },
                    { "GPU0000001", "RTX 4080 Super",                "High-end gaming graphics card with 16 GB GDDR6X",         2, 2 },
                    { "RAM0000001", "Corsair Vengeance DDR5 16GB",   "DDR5 RAM module 16GB 5200 MHz",                           3, 3 },
                    { "SSD0000001", "ASUS ROG Strix SSD 1TB",        "NVMe PCIe 4.0 SSD 1TB with 7000 MB/s read speed",        4, 4 }
                });

            // ── Seed: PCs ─────────────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "PCs",
                columns: new[] { "Id", "Name", "Weight", "Warranty", "CreatedAt", "Stock" },
                values: new object[,]
                {
                    { 1, "Gaming Beast X",    12.5, 36, new DateTime(2026, 5,  8, 9,  0, 0), 5  },
                    { 2, "Office Mini Pro",    4.2, 24, new DateTime(2026, 4, 15, 13, 30, 0), 12 },
                    { 3, "Workstation Ultra",  9.8, 48, new DateTime(2026, 3,  1, 10,  0, 0), 3  }
                });

            // ── Seed: PCComponents ────────────────────────────────────────────
            migrationBuilder.InsertData(
                table: "PCComponents",
                columns: new[] { "PcId", "ComponentCode", "Amount" },
                values: new object[,]
                {
                    { 1, "CPU0000001", 1 },
                    { 1, "GPU0000001", 1 },
                    { 1, "RAM0000001", 2 },
                    { 2, "CPU0000001", 1 },
                    { 2, "RAM0000001", 1 },
                    { 2, "SSD0000001", 1 },
                    { 3, "CPU0000001", 2 },
                    { 3, "RAM0000001", 4 },
                    { 3, "SSD0000001", 2 }
                });

            // ── Indexes ───────────────────────────────────────────────────────
            migrationBuilder.CreateIndex(
                name: "IX_Components_ComponentManufacturersId",
                table: "Components",
                column: "ComponentManufacturersId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_ComponentTypesId",
                table: "Components",
                column: "ComponentTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_PCComponents_ComponentCode",
                table: "PCComponents",
                column: "ComponentCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PCComponents");
            migrationBuilder.DropTable(name: "Components");
            migrationBuilder.DropTable(name: "PCs");
            migrationBuilder.DropTable(name: "ComponentTypes");
            migrationBuilder.DropTable(name: "ComponentManufacturers");
        }
    }
}
