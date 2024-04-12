namespace ParsingProject.BLL.Interfaces;

public interface IParsingService
{
    Task ParseChannelsDataAsync();

    Task UpdateChannelsDataAsync();
}