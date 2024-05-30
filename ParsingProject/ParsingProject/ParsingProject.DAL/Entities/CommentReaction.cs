namespace ParsingProject.DAL.Entities;

public class CommentReaction
{
    public long Id { get; set; }
    
    public long CommentId { get; set; }
    
    public Comment Comment { get; set; } = null!;

    public long? ReactionId { get; set; }

    public Reaction? Reaction { get; set; } = null!;

    public DateTime ParsedAt { get; set; }
    
    public int Count { get; set; }
}