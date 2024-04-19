﻿namespace ParsingProject.DAL.Entities;

public class Post
{
    public long Id { get; set; }
    
    public long TelegramId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int ViewsCount { get; set; }
    
    public DateTime Date { get; set; }
    
    public DateTime EditDate { get; set; }
    
    public DateTime ParsedAt { get; set; }
    
    public long ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;
    
    public ICollection<Comment> Comments { get; set; }
    
    public ICollection<PostReaction> Reactions { get; set; }
}