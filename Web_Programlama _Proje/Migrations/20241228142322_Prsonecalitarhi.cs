using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class Prsonecalitarhi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "PersonelCalismaSaati",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "PersonelCalismaSaati");
        }
    }
}
