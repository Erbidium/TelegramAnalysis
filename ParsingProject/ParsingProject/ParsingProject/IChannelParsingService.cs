namespace ParsingProject;

public interface IChannelParsingService
{
    Task ParseChannelsDataAsync(WTelegramService wt);

    Task UpdateChannelsDataAsync(WTelegramService wt);
}