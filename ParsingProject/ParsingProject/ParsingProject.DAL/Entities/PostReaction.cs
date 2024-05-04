namespace ParsingProject.DAL.Entities;

public class PostReaction
{
    public long Id { get; set; }
    
    public long PostId { get; set; }
    
    public Post Post { get; set; } = null!;

    public long? ReactionId { get; set; }

    public Reaction? Reaction { get; set; } = null!;

    public DateTime ParsedAt { get; set; }
    
    public int Count { get; set; }
}