using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class baslangic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rendevular",
                columns: table => new
                {
                    MusteriiID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusteriAd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MusteriSoyAd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MusteriTelefonNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MusteriMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RendevuZaman = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RendevuOnayDurumu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rendevular", x => x.MusteriiID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rendevular");
        }
    }
}
