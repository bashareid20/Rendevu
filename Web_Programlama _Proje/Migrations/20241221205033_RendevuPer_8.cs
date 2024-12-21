using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class RendevuPer_8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rendevular_Personel_PersonelID",
                table: "Rendevular");

            migrationBuilder.AlterColumn<int>(
                name: "PersonelID",
                table: "Rendevular",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Rendevular_Personel_PersonelID",
                table: "Rendevular",
                column: "PersonelID",
                principalTable: "Personel",
                principalColumn: "PersonelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rendevular_Personel_PersonelID",
                table: "Rendevular");

            migrationBuilder.AlterColumn<int>(
                name: "PersonelID",
                table: "Rendevular",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
