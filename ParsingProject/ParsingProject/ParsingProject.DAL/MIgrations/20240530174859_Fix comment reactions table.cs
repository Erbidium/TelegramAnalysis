using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsingProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fixcommentreactionstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "emoticon",
                table: "commentreactions");

            migrationBuilder.AddColumn<long>(
                name: "reactionid",
                table: "commentreactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_commentreactions_reactionid",
                table: "commentreactions",
                column: "reactionid");

            migrationBuilder.AddForeignKey(
                name: "fk_commentreactions_reactions_reactionid",
                table: "commentreactions",
                column: "reactionid",
                principalTable: "reactions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_commentreactions_reactions_reactionid",
                table: "commentreactions");

            migrationBuilder.DropIndex(
                name: "ix_commentreactions_reactionid",
                table: "commentreactions");

            migrationBuilder.DropColumn(
                name: "reactionid",
                table: "commentreactions");

            migrationBuilder.AddColumn<string>(
                name: "emoticon",
                table: "commentreactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
