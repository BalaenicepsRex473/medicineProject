using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scrubsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Dictionaryupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "parentId",
                table: "Icd10s",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "parentId",
                table: "Icd10s");
        }
    }
}
