using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsingProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Storecommenthash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Hash",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Comments");
        }
    }
}
