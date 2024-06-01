using WTelegram;

namespace ParsingProject.BLL.Interfaces;

public interface IChannelService
{
    List<DAL.Entities.Channel> GetChannelsToParse();

    Task<bool> SaveChannel(string channelLink, Client client);

    Task<bool> DeleteChannel(long id);
}