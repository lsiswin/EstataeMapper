using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EstateMapperWeb.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    RegionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.RegionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubRegions",
                columns: table => new
                {
                    SubRegionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubRegions", x => x.SubRegionId);
                    table.ForeignKey(
                        name: "FK_SubRegions_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "House",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MainImageUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubRegionId = table.Column<int>(type: "int", nullable: false),
                    DetailAddress = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_House", x => x.Id);
                    table.ForeignKey(
                        name: "FK_House_SubRegions_SubRegionId",
                        column: x => x.SubRegionId,
                        principalTable: "SubRegions",
                        principalColumn: "SubRegionId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Layouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LayoutName = table.Column<int>(type: "int", nullable: false),
                    LayoutUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HouseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Layouts_House_HouseId",
                        column: x => x.HouseId,
                        principalTable: "House",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TagName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HouseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_House_HouseId",
                        column: x => x.HouseId,
                        principalTable: "House",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "RegionId", "Name" },
                values: new object[,]
                {
                    { 420100, "武汉市" },
                    { 420200, "黄石市" },
                    { 420300, "十堰市" },
                    { 420500, "宜昌市" },
                    { 420600, "襄阳市" },
                    { 420700, "鄂州市" },
                    { 420800, "荆门市" },
                    { 420900, "孝感市" },
                    { 421000, "荆州市" },
                    { 421100, "黄冈市" },
                    { 421200, "咸宁市" },
                    { 421300, "随州市" },
                    { 422800, "恩施土家族苗族自治州" },
                    { 429004, "仙桃市" },
                    { 429005, "潜江市" },
                    { 429006, "天门市" },
                    { 429021, "神农架林区" }
                });

            migrationBuilder.InsertData(
                table: "SubRegions",
                columns: new[] { "SubRegionId", "Name", "RegionId" },
                values: new object[,]
                {
                    { 420101, "江岸区", 420100 },
                    { 420102, "江汉区", 420100 },
                    { 420103, "硚口区", 420100 },
                    { 420104, "汉阳区", 420100 },
                    { 420105, "武昌区", 420100 },
                    { 420106, "青山区", 420100 },
                    { 420107, "洪山区", 420100 },
                    { 420108, "东西湖区", 420100 },
                    { 420109, "汉南区", 420100 },
                    { 420110, "蔡甸区", 420100 },
                    { 420111, "江夏区", 420100 },
                    { 420112, "黄陂区", 420100 },
                    { 420113, "新洲区", 420100 },
                    { 420201, "黄石港区", 420200 },
                    { 420202, "西塞山区", 420200 },
                    { 420203, "下陆区", 420200 },
                    { 420204, "铁山区", 420200 },
                    { 420205, "阳新县", 420200 },
                    { 420206, "大冶市", 420200 },
                    { 429004, "仙桃市", 429004 },
                    { 429005, "潜江市", 429005 },
                    { 429006, "天门市", 429006 },
                    { 429021, "神农架林区", 429021 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_House_SubRegionId",
                table: "House",
                column: "SubRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Layouts_HouseId",
                table: "Layouts",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SubRegions_RegionId",
                table: "SubRegions",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_HouseId",
                table: "Tags",
                column: "HouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Layouts");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "House");

            migrationBuilder.DropTable(
                name: "SubRegions");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
