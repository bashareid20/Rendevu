using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class RendevuPer_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rendevular_Personel_PersonelID",
                table: "Rendevular");

            migrationBuilder.DropTable(
                name: "Personel");

            migrationBuilder.DropIndex(
                name: "IX_Rendevular_PersonelID",
                table: "Rendevular");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personel",
                columns: table => new
                {
                    PersonelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonelAd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonelSoyAd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonelYetenekleri = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personel", x => x.PersonelID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rendevular_PersonelID",
                table: "Rendevular",
                column: "PersonelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rendevular_Personel_PersonelID",
                table: "Rendevular",
                column: "PersonelID",
                principalTable: "Personel",
                principalColumn: "PersonelID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
