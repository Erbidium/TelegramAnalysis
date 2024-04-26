namespace ParsingProject.DAL.Entities;

public class CommentReaction
{
    public long Id { get; set; }
    
    public long CommentId { get; set; }
    
    public Comment Comment { get; set; } = null!;

    public string Emoticon { get; set; } = string.Empty;

    public string Reaction { get; set; } = string.Empty;

    public DateTime ParsedAt { get; set; }
    
    public int Count { get; set; }
}