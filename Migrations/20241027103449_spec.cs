using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scrubsAPI.Migrations
{
    /// <inheritdoc />
    public partial class spec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "speciality",
                table: "Doctors",
                newName: "specialityid");

            migrationBuilder.CreateTable(
                name: "BannedTokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    token = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedTokens", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_specialityid",
                table: "Doctors",
                column: "specialityid");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Specialities_specialityid",
                table: "Doctors",
                column: "specialityid",
                principalTable: "Specialities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Specialities_specialityid",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "BannedTokens");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_specialityid",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "specialityid",
                table: "Doctors",
                newName: "speciality");
        }
    }
}
