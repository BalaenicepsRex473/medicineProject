using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scrubsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Icd10Parse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idFromJson",
                table: "Icd10s",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Icd10s_parentId",
                table: "Icd10s",
                column: "parentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Icd10s_Icd10s_parentId",
                table: "Icd10s",
                column: "parentId",
                principalTable: "Icd10s",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Icd10s_Icd10s_parentId",
                table: "Icd10s");

            migrationBuilder.DropIndex(
                name: "IX_Icd10s_parentId",
                table: "Icd10s");

            migrationBuilder.DropColumn(
                name: "idFromJson",
                table: "Icd10s");
        }
    }
}
