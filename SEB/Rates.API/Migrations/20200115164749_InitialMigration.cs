using Microsoft.EntityFrameworkCore.Migrations;

namespace Rates.API.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Agreements",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    BaseRateCode = table.Column<string>(nullable: false),
                    NewBaseRateCode = table.Column<string>(nullable: true),
                    Margin = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    UserId = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agreements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { 67812203006m, "Goras", "Trusevičius" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { 78706151287m, "Dangė", "Kulkavičiutė" });

            migrationBuilder.InsertData(
                table: "Agreements",
                columns: new[] { "Id", "Amount", "BaseRateCode", "Duration", "Margin", "NewBaseRateCode", "UserId" },
                values: new object[] { 1m, 12000, "VILIBOR3m", 60, 1.6m, null, 67812203006m });

            migrationBuilder.InsertData(
                table: "Agreements",
                columns: new[] { "Id", "Amount", "BaseRateCode", "Duration", "Margin", "NewBaseRateCode", "UserId" },
                values: new object[] { 2m, 8000, "VILIBOR1y", 36, 2.2m, null, 78706151287m });

            migrationBuilder.InsertData(
                table: "Agreements",
                columns: new[] { "Id", "Amount", "BaseRateCode", "Duration", "Margin", "NewBaseRateCode", "UserId" },
                values: new object[] { 3m, 1000, "VILIBOR6m", 24, 1.85m, null, 78706151287m });

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_UserId",
                table: "Agreements",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agreements");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
