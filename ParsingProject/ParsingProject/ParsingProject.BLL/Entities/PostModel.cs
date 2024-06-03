﻿namespace ParsingProject.BLL.Entities;

public class PostModel
{
    public long Id { get; set; }
    
    public long ChannelId { get; set; }

    public long TelegramId { get; set; }
    
    public string Text { get; set; } = string.Empty;

    public int Hash { get; set; }
    
    public int ViewsCount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime EditedAt { get; set; }
    
    public DateTime ParsedAt { get; set; }
}