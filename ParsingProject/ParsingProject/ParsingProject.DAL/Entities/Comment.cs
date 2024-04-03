namespace ParsingProject.DAL.Entities;

public class Comment
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public long PostId { get; set; }
    public Post Post { get; set; }
}