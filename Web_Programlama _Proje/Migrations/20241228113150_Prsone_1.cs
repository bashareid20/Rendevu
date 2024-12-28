using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class Prsone_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonelCalismaSaatleri",
                table: "Personaller",
                newName: "CalismaSaati");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CalismaSaati",
                table: "Personaller",
                newName: "PersonelCalismaSaatleri");
        }
    }
}
