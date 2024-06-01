using TL;
using WTelegram;

namespace ParsingProject.BLL.Interfaces;

public interface IPostService
{
    Task SavePostDataAsync(Message message, Channel channel, long channelId, Client client);
}