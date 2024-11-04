using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scrubsAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixes4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "notified",
                table: "Inspections",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notified",
                table: "Inspections");
        }
    }
}
