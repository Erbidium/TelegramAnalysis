namespace ParsingProject.DAL.Entities;

public class PostReaction
{
    public long Id { get; set; }
    
    public long PostId { get; set; }

    public string Emoticon { get; set; } = string.Empty;

    public string Reaction { get; set; } = string.Empty;
    
    public int Count { get; set; }
}