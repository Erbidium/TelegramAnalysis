namespace ParsingProject.DAL.Entities;

public class Channel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Post> Posts { get; set; }
}