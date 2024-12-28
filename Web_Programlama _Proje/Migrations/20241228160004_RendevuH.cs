using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class RendevuH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RendevuHizmet",
                columns: table => new
                {
                    RendevuID = table.Column<int>(type: "int", nullable: false),
                    HizmetID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RendevuHizmet", x => new { x.RendevuID, x.HizmetID });
                    table.ForeignKey(
                        name: "FK_RendevuHizmet_Hizmetler_HizmetID",
                        column: x => x.HizmetID,
                        principalTable: "Hizmetler",
                        principalColumn: "HizmetID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RendevuHizmet_Rendevular_RendevuID",
                        column: x => x.RendevuID,
                        principalTable: "Rendevular",
                        principalColumn: "MusteriiID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RendevuHizmet_HizmetID",
                table: "RendevuHizmet",
                column: "HizmetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RendevuHizmet");
        }
    }
}
