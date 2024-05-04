using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ParsingProject.DAL.Entities;

namespace ParsingProject.DAL.Context;

public class ParsingProjectContext : DbContext
{
    public DbSet<Channel> Channels { get; private set; }
    public DbSet<Post> Posts { get; private set; }
    public DbSet<PostReaction> PostReactions { get; private set; }
    public DbSet<Comment> Comments { get; private set; }
    public DbSet<CommentReaction> CommentReactions { get; private set; }
    
    public DbSet<Reaction> Reactions { get; private set; }

    public ParsingProjectContext(DbContextOptions<ParsingProjectContext> options) : base(options)
    {
        Channels = Set<Channel>();
        Posts = Set<Post>();
        PostReactions = Set<PostReaction>();
        Comments = Set<Comment>();
        CommentReactions = Set<CommentReaction>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .Entity<Channel>()   
            .HasMany(c => c.Posts)
            .WithOne(p => p.Channel)
            .HasForeignKey(p => p.ChannelId);
        
        builder
            .Entity<Post>()   
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId);

        builder
            .Entity<Post>()
            .HasMany(p => p.Reactions)
            .WithOne(r => r.Post)
            .HasForeignKey(r => r.PostId);
        
        builder
            .Entity<Comment>()
            .HasMany(c => c.Reactions)
            .WithOne(r => r.Comment)
            .HasForeignKey(r => r.CommentId);
    }
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ParsingProjectContext>
{
    public ParsingProjectContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@Directory.GetCurrentDirectory() + "/../ParsingProject/appsettings.json")
            .Build();
        var builder = new DbContextOptionsBuilder<ParsingProjectContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.UseSqlServer(connectionString);
        builder.UseLowerCaseNamingConvention();
        return new ParsingProjectContext(builder.Options);
    }
}