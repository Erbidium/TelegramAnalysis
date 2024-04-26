namespace ParsingProject.DAL.Entities;

public class PostReaction
{
    public long Id { get; set; }
    
    public long PostId { get; set; }
    
    public Post Post { get; set; } = null!;

    public string Emoticon { get; set; } = string.Empty;

    public string Reaction { get; set; } = string.Empty;

    public DateTime ParsedAt { get; set; }
    
    public int Count { get; set; }
}