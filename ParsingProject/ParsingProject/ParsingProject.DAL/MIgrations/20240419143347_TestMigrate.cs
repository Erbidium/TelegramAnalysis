using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsingProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TestMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Channels",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "MainUsername",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "TelegramId",
                table: "Channels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainUsername",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "TelegramId",
                table: "Channels");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Channels",
                newName: "Name");
        }
    }
}
