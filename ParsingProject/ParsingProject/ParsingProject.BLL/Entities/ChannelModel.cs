namespace ParsingProject.BLL.Entities;

public class ChannelModel
{
    public long Id { get; set; }
    
    public long TelegramId { get; set; }
    
    public int ParticipantsCount { get; set; }
    
    public string? MainUsername { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}