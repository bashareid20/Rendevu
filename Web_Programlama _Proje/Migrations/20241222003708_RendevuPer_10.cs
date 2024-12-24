using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama__Proje.Migrations
{
    public partial class RendevuPer_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rendevular_Personel_PersonelID",
                table: "Rendevular");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personel",
                table: "Personel");

            migrationBuilder.RenameTable(
                name: "Personel",
                newName: "Personaller");

            migrationBuilder.AlterColumn<int>(
                name: "PersonelID",
                table: "Rendevular",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personaller",
                table: "Personaller",
                column: "PersonelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rendevular_Personaller_PersonelID",
                table: "Rendevular",
                column: "PersonelID",
                principalTable: "Personaller",
                principalColumn: "PersonelID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rendevular_Personaller_PersonelID",
                table: "Rendevular");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personaller",
                table: "Personaller");

            migrationBuilder.RenameTable(
                name: "Personaller",
                newName: "Personel");

            migrationBuilder.AlterColumn<int>(
                name: "PersonelID",
                table: "Rendevular",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personel",
                table: "Personel",
                column: "PersonelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rendevular_Personel_PersonelID",
                table: "Rendevular",
                column: "PersonelID",
                principalTable: "Personel",
                principalColumn: "PersonelID");
        }
    }
}
