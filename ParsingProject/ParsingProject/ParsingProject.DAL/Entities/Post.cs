namespace ParsingProject.DAL.Entities;

public class Post
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int ViewsCount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public long ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;
    
    public ICollection<Comment> Comments { get; set; }
    //public List<int> Reactions { get; set; }
}