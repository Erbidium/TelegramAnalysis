namespace ParsingProject.DAL.Entities;

public class Channel
{
    public long Id { get; set; }
    
    public long TelegramId { get; set; }
    
    public string? MainUsername { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    
    public ICollection<Post> Posts { get; set; }
}