namespace ParsingProject;

public interface IParsingService
{
    Task ParseChannelsDataAsync(WTelegramService wt);

    Task UpdateChannelsDataAsync(WTelegramService wt);
}