using Microsoft.EntityFrameworkCore;
using ParsingProject.DAL.Entities;

namespace ParsingProject.DAL.Context;

public class ParsingProjectContext : DbContext
{
    public DbSet<Channel> Channels { get; private set; }
    public DbSet<Post> Posts { get; private set; }
    public DbSet<Comment> Comments { get; private set; }
    
    public ParsingProjectContext(DbContextOptions<ParsingProjectContext> options) : base(options)
    {
        Channels = Set<Channel>();
        Posts = Set<Post>();
        Comments = Set<Comment>();
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
    }
}