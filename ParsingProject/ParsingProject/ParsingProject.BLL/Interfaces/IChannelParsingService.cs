using WTelegram;

namespace ParsingProject.BLL.Interfaces;

public interface IChannelParsingService
{
    Task ParseChannelsDataAsync(Client client, DateTime parsingDate, CancellationToken cancellationToken);

    Task UpdateChannelsDataAsync(Client client);
}