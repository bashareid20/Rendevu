using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class Prsonecalismasaatleri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalismaSaati",
                table: "Personaller");

            migrationBuilder.CreateTable(
                name: "PersonelCalismaSaati",
                columns: table => new
                {
                    CalismaSaatiID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonelID = table.Column<int>(type: "int", nullable: false),
                    CalismaSaati = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelCalismaSaati", x => x.CalismaSaatiID);
                    table.ForeignKey(
                        name: "FK_PersonelCalismaSaati_Personaller_PersonelID",
                        column: x => x.PersonelID,
                        principalTable: "Personaller",
                        principalColumn: "PersonelID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonelCalismaSaati_PersonelID",
                table: "PersonelCalismaSaati",
                column: "PersonelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonelCalismaSaati");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CalismaSaati",
                table: "Personaller",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
