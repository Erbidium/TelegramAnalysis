namespace ParsingProject;

public interface IChannelParsingService
{
    Task ParseChannelsDataAsync(WTelegramService wt, DateTime parsingDate, CancellationToken cancellationToken);

    Task UpdateChannelsDataAsync(WTelegramService wt);
}