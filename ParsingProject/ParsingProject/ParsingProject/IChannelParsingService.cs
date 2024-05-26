namespace ParsingProject;

public interface IChannelParsingService
{
    Task ParseChannelsDataAsync(WTelegramService wt, CancellationToken cancellationToken);

    Task UpdateChannelsDataAsync(WTelegramService wt);
}