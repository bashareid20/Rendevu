using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class Prsone_calismaSaatleri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "PersonelCalismaSaatleri",
                table: "Personaller",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonelCalismaSaatleri",
                table: "Personaller");
        }
    }
}
