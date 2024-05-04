using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsingProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Createseparatereactionstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "emoticon",
                table: "postreactions");

            migrationBuilder.AddColumn<long>(
                name: "reactionid",
                table: "postreactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "reactions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    emoticon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reactions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_postreactions_reactionid",
                table: "postreactions",
                column: "reactionid");

            migrationBuilder.AddForeignKey(
                name: "fk_postreactions_reactions_reactionid",
                table: "postreactions",
                column: "reactionid",
                principalTable: "reactions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_postreactions_reactions_reactionid",
                table: "postreactions");

            migrationBuilder.DropTable(
                name: "reactions");

            migrationBuilder.DropIndex(
                name: "ix_postreactions_reactionid",
                table: "postreactions");

            migrationBuilder.DropColumn(
                name: "reactionid",
                table: "postreactions");

            migrationBuilder.AddColumn<string>(
                name: "emoticon",
                table: "postreactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
