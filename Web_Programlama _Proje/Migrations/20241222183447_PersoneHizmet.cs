using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class PersoneHizmet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hizmetler",
                columns: table => new
                {
                    HizmetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HizmetUcreti = table.Column<double>(type: "float", nullable: false),
                    HizmetAd = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HizmetSuresi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hizmetler", x => x.HizmetID);
                });

            migrationBuilder.CreateTable(
                name: "PersonelHizmetler",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonelID = table.Column<int>(type: "int", nullable: false),
                    HizmetID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelHizmetler", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PersonelHizmetler_Hizmetler_HizmetID",
                        column: x => x.HizmetID,
                        principalTable: "Hizmetler",
                        principalColumn: "HizmetID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonelHizmetler_Personaller_PersonelID",
                        column: x => x.PersonelID,
                        principalTable: "Personaller",
                        principalColumn: "PersonelID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonelHizmetler_HizmetID",
                table: "PersonelHizmetler",
                column: "HizmetID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelHizmetler_PersonelID",
                table: "PersonelHizmetler",
                column: "PersonelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonelHizmetler");

            migrationBuilder.DropTable(
                name: "Hizmetler");
        }
    }
}
