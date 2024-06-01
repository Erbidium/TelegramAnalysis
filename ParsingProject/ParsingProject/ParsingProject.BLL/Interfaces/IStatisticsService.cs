using ParsingProject.BLL.Entities;

namespace ParsingProject.BLL.Interfaces;

public interface IStatisticsService
{
    public ParsingStatisticsModel GetParsingStatistics();
}