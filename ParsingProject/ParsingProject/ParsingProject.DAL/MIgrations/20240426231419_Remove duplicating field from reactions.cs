using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsingProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Removeduplicatingfieldfromreactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reaction",
                table: "postreactions");

            migrationBuilder.DropColumn(
                name: "reaction",
                table: "commentreactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "reaction",
                table: "postreactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "reaction",
                table: "commentreactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
