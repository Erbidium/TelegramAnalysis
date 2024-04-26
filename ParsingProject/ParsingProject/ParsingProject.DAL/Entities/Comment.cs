namespace ParsingProject.DAL.Entities;

public class Comment
{
    public long Id { get; set; }
    
    public long PostId { get; set; }
    public Post Post { get; set; } = null!;
    
    public ICollection<CommentReaction> Reactions { get; set; }
    
    public long TelegramId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Hash { get; set; }
    
    public int ViewsCount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime EditedAt { get; set; }
    
    public DateTime ParsedAt { get; set; }
}