using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsingProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Lowercasecolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReactions_Comments_CommentId",
                table: "CommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactions_Posts_PostId",
                table: "PostReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Channels_ChannelId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostReactions",
                table: "PostReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentReactions",
                table: "CommentReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channels",
                table: "Channels");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "posts");

            migrationBuilder.RenameTable(
                name: "PostReactions",
                newName: "postreactions");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "comments");

            migrationBuilder.RenameTable(
                name: "CommentReactions",
                newName: "commentreactions");

            migrationBuilder.RenameTable(
                name: "Channels",
                newName: "channels");

            migrationBuilder.RenameColumn(
                name: "ViewsCount",
                table: "posts",
                newName: "viewscount");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "posts",
                newName: "text");

            migrationBuilder.RenameColumn(
                name: "TelegramId",
                table: "posts",
                newName: "telegramid");

            migrationBuilder.RenameColumn(
                name: "ParsedAt",
                table: "posts",
                newName: "parsedat");

            migrationBuilder.RenameColumn(
                name: "Hash",
                table: "posts",
                newName: "hash");

            migrationBuilder.RenameColumn(
                name: "EditDate",
                table: "posts",
                newName: "editdate");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "posts",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "ChannelId",
                table: "posts",
                newName: "channelid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "posts",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_ChannelId",
                table: "posts",
                newName: "ix_posts_channelid");

            migrationBuilder.RenameColumn(
                name: "Reaction",
                table: "postreactions",
                newName: "reaction");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "postreactions",
                newName: "postid");

            migrationBuilder.RenameColumn(
                name: "ParsedAt",
                table: "postreactions",
                newName: "parsedat");

            migrationBuilder.RenameColumn(
                name: "Emoticon",
                table: "postreactions",
                newName: "emoticon");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "postreactions",
                newName: "count");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "postreactions",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_PostReactions_PostId",
                table: "postreactions",
                newName: "ix_postreactions_postid");

            migrationBuilder.RenameColumn(
                name: "ViewsCount",
                table: "comments",
                newName: "viewscount");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "comments",
                newName: "text");

            migrationBuilder.RenameColumn(
                name: "TelegramId",
                table: "comments",
                newName: "telegramid");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "comments",
                newName: "postid");

            migrationBuilder.RenameColumn(
                name: "ParsedAt",
                table: "comments",
                newName: "parsedat");

            migrationBuilder.RenameColumn(
                name: "Hash",
                table: "comments",
                newName: "hash");

            migrationBuilder.RenameColumn(
                name: "EditDate",
                table: "comments",
                newName: "editdate");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "comments",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "comments",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "comments",
                newName: "ix_comments_postid");

            migrationBuilder.RenameColumn(
                name: "Reaction",
                table: "commentreactions",
                newName: "reaction");

            migrationBuilder.RenameColumn(
                name: "ParsedAt",
                table: "commentreactions",
                newName: "parsedat");

            migrationBuilder.RenameColumn(
                name: "Emoticon",
                table: "commentreactions",
                newName: "emoticon");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "commentreactions",
                newName: "count");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "commentreactions",
                newName: "commentid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "commentreactions",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_CommentReactions_CommentId",
                table: "commentreactions",
                newName: "ix_commentreactions_commentid");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "channels",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "TelegramId",
                table: "channels",
                newName: "telegramid");

            migrationBuilder.RenameColumn(
                name: "ParticipantsCount",
                table: "channels",
                newName: "participantscount");

            migrationBuilder.RenameColumn(
                name: "MainUsername",
                table: "channels",
                newName: "mainusername");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "channels",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_posts",
                table: "posts",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_postreactions",
                table: "postreactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_comments",
                table: "comments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_commentreactions",
                table: "commentreactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_channels",
                table: "channels",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_commentreactions_comments_commentid",
                table: "commentreactions",
                column: "commentid",
                principalTable: "comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_comments_posts_postid",
                table: "comments",
                column: "postid",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_postreactions_posts_postid",
                table: "postreactions",
                column: "postid",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_posts_channels_channelid",
                table: "posts",
                column: "channelid",
                principalTable: "channels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_commentreactions_comments_commentid",
                table: "commentreactions");

            migrationBuilder.DropForeignKey(
                name: "fk_comments_posts_postid",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "fk_postreactions_posts_postid",
                table: "postreactions");

            migrationBuilder.DropForeignKey(
                name: "fk_posts_channels_channelid",
                table: "posts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_posts",
                table: "posts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_postreactions",
                table: "postreactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_comments",
                table: "comments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_commentreactions",
                table: "commentreactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_channels",
                table: "channels");

            migrationBuilder.RenameTable(
                name: "posts",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "postreactions",
                newName: "PostReactions");

            migrationBuilder.RenameTable(
                name: "comments",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "commentreactions",
                newName: "CommentReactions");

            migrationBuilder.RenameTable(
                name: "channels",
                newName: "Channels");

            migrationBuilder.RenameColumn(
                name: "viewscount",
                table: "Posts",
                newName: "ViewsCount");

            migrationBuilder.RenameColumn(
                name: "text",
                table: "Posts",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "telegramid",
                table: "Posts",
                newName: "TelegramId");

            migrationBuilder.RenameColumn(
                name: "parsedat",
                table: "Posts",
                newName: "ParsedAt");

            migrationBuilder.RenameColumn(
                name: "hash",
                table: "Posts",
                newName: "Hash");

            migrationBuilder.RenameColumn(
                name: "editdate",
                table: "Posts",
                newName: "EditDate");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Posts",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "channelid",
                table: "Posts",
                newName: "ChannelId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Posts",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_posts_channelid",
                table: "Posts",
                newName: "IX_Posts_ChannelId");

            migrationBuilder.RenameColumn(
                name: "reaction",
                table: "PostReactions",
                newName: "Reaction");

            migrationBuilder.RenameColumn(
                name: "postid",
                table: "PostReactions",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "parsedat",
                table: "PostReactions",
                newName: "ParsedAt");

            migrationBuilder.RenameColumn(
                name: "emoticon",
                table: "PostReactions",
                newName: "Emoticon");

            migrationBuilder.RenameColumn(
                name: "count",
                table: "PostReactions",
                newName: "Count");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PostReactions",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_postreactions_postid",
                table: "PostReactions",
                newName: "IX_PostReactions_PostId");

            migrationBuilder.RenameColumn(
                name: "viewscount",
                table: "Comments",
                newName: "ViewsCount");

            migrationBuilder.RenameColumn(
                name: "text",
                table: "Comments",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "telegramid",
                table: "Comments",
                newName: "TelegramId");

            migrationBuilder.RenameColumn(
                name: "postid",
                table: "Comments",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "parsedat",
                table: "Comments",
                newName: "ParsedAt");

            migrationBuilder.RenameColumn(
                name: "hash",
                table: "Comments",
                newName: "Hash");

            migrationBuilder.RenameColumn(
                name: "editdate",
                table: "Comments",
                newName: "EditDate");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Comments",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Comments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_comments_postid",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.RenameColumn(
                name: "reaction",
                table: "CommentReactions",
                newName: "Reaction");

            migrationBuilder.RenameColumn(
                name: "parsedat",
                table: "CommentReactions",
                newName: "ParsedAt");

            migrationBuilder.RenameColumn(
                name: "emoticon",
                table: "CommentReactions",
                newName: "Emoticon");

            migrationBuilder.RenameColumn(
                name: "count",
                table: "CommentReactions",
                newName: "Count");

            migrationBuilder.RenameColumn(
                name: "commentid",
                table: "CommentReactions",
                newName: "CommentId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CommentReactions",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_commentreactions_commentid",
                table: "CommentReactions",
                newName: "IX_CommentReactions_CommentId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Channels",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "telegramid",
                table: "Channels",
                newName: "TelegramId");

            migrationBuilder.RenameColumn(
                name: "participantscount",
                table: "Channels",
                newName: "ParticipantsCount");

            migrationBuilder.RenameColumn(
                name: "mainusername",
                table: "Channels",
                newName: "MainUsername");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Channels",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostReactions",
                table: "PostReactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentReactions",
                table: "CommentReactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channels",
                table: "Channels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReactions_Comments_CommentId",
                table: "CommentReactions",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactions_Posts_PostId",
                table: "PostReactions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Channels_ChannelId",
                table: "Posts",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
