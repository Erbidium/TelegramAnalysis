﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParsingProject.DAL.Context;

#nullable disable

namespace ParsingProject.DAL.Migrations
{
    [DbContext(typeof(ParsingProjectContext))]
    partial class ParsingProjectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ParsingProject.DAL.Entities.Channel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasColumnName("isdeleted");

                    b.Property<string>("MainUsername")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("mainusername");

                    b.Property<int>("ParticipantsCount")
                        .HasColumnType("int")
                        .HasColumnName("participantscount");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint")
                        .HasColumnName("telegramid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_channels");

                    b.ToTable("channels", (string)null);
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("createdat");

                    b.Property<DateTime>("EditedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("editedat");

                    b.Property<int>("Hash")
                        .HasColumnType("int")
                        .HasColumnName("hash");

                    b.Property<DateTime>("ParsedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("parsedat");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint")
                        .HasColumnName("postid");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint")
                        .HasColumnName("telegramid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("text");

                    b.Property<int>("ViewsCount")
                        .HasColumnType("int")
                        .HasColumnName("viewscount");

                    b.HasKey("Id")
                        .HasName("pk_comments");

                    b.HasIndex("PostId")
                        .HasDatabaseName("ix_comments_postid");

                    b.ToTable("comments", (string)null);
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.CommentReaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CommentId")
                        .HasColumnType("bigint")
                        .HasColumnName("commentid");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.Property<string>("Emoticon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("emoticon");

                    b.Property<DateTime>("ParsedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("parsedat");

                    b.HasKey("Id")
                        .HasName("pk_commentreactions");

                    b.HasIndex("CommentId")
                        .HasDatabaseName("ix_commentreactions_commentid");

                    b.ToTable("commentreactions", (string)null);
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ChannelId")
                        .HasColumnType("bigint")
                        .HasColumnName("channelid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("createdat");

                    b.Property<DateTime>("EditedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("editedat");

                    b.Property<int>("Hash")
                        .HasColumnType("int")
                        .HasColumnName("hash");

                    b.Property<DateTime>("ParsedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("parsedat");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint")
                        .HasColumnName("telegramid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("text");

                    b.Property<int>("ViewsCount")
                        .HasColumnType("int")
                        .HasColumnName("viewscount");

                    b.HasKey("Id")
                        .HasName("pk_posts");

                    b.HasIndex("ChannelId")
                        .HasDatabaseName("ix_posts_channelid");

                    b.ToTable("posts", (string)null);
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.PostReaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.Property<DateTime>("ParsedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("parsedat");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint")
                        .HasColumnName("postid");

                    b.Property<long?>("ReactionId")
                        .HasColumnType("bigint")
                        .HasColumnName("reactionid");

                    b.HasKey("Id")
                        .HasName("pk_postreactions");

                    b.HasIndex("PostId")
                        .HasDatabaseName("ix_postreactions_postid");

                    b.HasIndex("ReactionId")
                        .HasDatabaseName("ix_postreactions_reactionid");

                    b.ToTable("postreactions", (string)null);
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.Reaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Emoticon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("emoticon");

                    b.HasKey("Id")
                        .HasName("pk_reactions");

                    b.ToTable("reactions", (string)null);
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.Comment", b =>
                {
                    b.HasOne("ParsingProject.DAL.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comments_posts_postid");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.CommentReaction", b =>
                {
                    b.HasOne("ParsingProject.DAL.Entities.Comment", "Comment")
                        .WithMany("Reactions")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_commentreactions_comments_commentid");

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.Post", b =>
                {
                    b.HasOne("ParsingProject.DAL.Entities.Channel", "Channel")
                        .WithMany("Posts")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_posts_channels_channelid");

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.PostReaction", b =>
                {
                    b.HasOne("ParsingProject.DAL.Entities.Post", "Post")
                        .WithMany("Reactions")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_postreactions_posts_postid");

                    b.HasOne("ParsingProject.DAL.Entities.Reaction", "Reaction")
                        .WithMany()
                        .HasForeignKey("ReactionId")
                        .HasConstraintName("fk_postreactions_reactions_reactionid");

                    b.Navigation("Post");

                    b.Navigation("Reaction");
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.Channel", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.Comment", b =>
                {
                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("ParsingProject.DAL.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Reactions");
                });
#pragma warning restore 612, 618
        }
    }
}
