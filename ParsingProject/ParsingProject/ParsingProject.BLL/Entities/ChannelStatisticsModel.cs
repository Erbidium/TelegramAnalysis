using ParsingProject.DAL.Entities;

namespace ParsingProject.BLL.Entities;

public class ChannelStatisticsModel
{
    public ChannelModel Channel { get; set; }
        
    public int ParsedPostsCount { get; set; }
        
    public int ParsedReactionsCount { get; set; }
    
    public int ParsedCommentsCount { get; set; }
        
    public DateTime? StartDate { get; set; }
        
    public DateTime? EndDate { get; set; }
}