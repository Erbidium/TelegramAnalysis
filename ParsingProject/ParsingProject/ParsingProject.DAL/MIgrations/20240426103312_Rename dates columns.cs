using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsingProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Renamedatescolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "editdate",
                table: "posts",
                newName: "editedat");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "posts",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "editdate",
                table: "comments",
                newName: "editedat");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "comments",
                newName: "createdat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "editedat",
                table: "posts",
                newName: "editdate");

            migrationBuilder.RenameColumn(
                name: "createdat",
                table: "posts",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "editedat",
                table: "comments",
                newName: "editdate");

            migrationBuilder.RenameColumn(
                name: "createdat",
                table: "comments",
                newName: "date");
        }
    }
}
