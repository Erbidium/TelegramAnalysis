using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsingProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedtoChannels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isdeleted",
                table: "channels",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isdeleted",
                table: "channels");
        }
    }
}
