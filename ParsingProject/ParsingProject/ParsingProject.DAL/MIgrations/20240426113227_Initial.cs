using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsingProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "channels",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    telegramid = table.Column<long>(type: "bigint", nullable: false),
                    participantscount = table.Column<int>(type: "int", nullable: false),
                    mainusername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_channels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    channelid = table.Column<long>(type: "bigint", nullable: false),
                    telegramid = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hash = table.Column<int>(type: "int", nullable: false),
                    viewscount = table.Column<int>(type: "int", nullable: false),
                    createdat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    editedat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    parsedat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_posts", x => x.id);
                    table.ForeignKey(
                        name: "fk_posts_channels_channelid",
                        column: x => x.channelid,
                        principalTable: "channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postid = table.Column<long>(type: "bigint", nullable: false),
                    telegramid = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hash = table.Column<int>(type: "int", nullable: false),
                    viewscount = table.Column<int>(type: "int", nullable: false),
                    createdat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    editedat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    parsedat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_comments_posts_postid",
                        column: x => x.postid,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "postreactions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postid = table.Column<long>(type: "bigint", nullable: false),
                    emoticon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reaction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parsedat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_postreactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_postreactions_posts_postid",
                        column: x => x.postid,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "commentreactions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    commentid = table.Column<long>(type: "bigint", nullable: false),
                    emoticon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reaction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parsedat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_commentreactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_commentreactions_comments_commentid",
                        column: x => x.commentid,
                        principalTable: "comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_commentreactions_commentid",
                table: "commentreactions",
                column: "commentid");

            migrationBuilder.CreateIndex(
                name: "ix_comments_postid",
                table: "comments",
                column: "postid");

            migrationBuilder.CreateIndex(
                name: "ix_postreactions_postid",
                table: "postreactions",
                column: "postid");

            migrationBuilder.CreateIndex(
                name: "ix_posts_channelid",
                table: "posts",
                column: "channelid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commentreactions");

            migrationBuilder.DropTable(
                name: "postreactions");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "channels");
        }
    }
}
