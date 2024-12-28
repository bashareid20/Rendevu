using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class Isdelete4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RendevuDurumu",
                table: "Rendevular",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RendevuDurumu",
                table: "Rendevular");
        }
    }
}
