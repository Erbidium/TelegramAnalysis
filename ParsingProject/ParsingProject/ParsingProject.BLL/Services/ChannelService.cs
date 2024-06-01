using AutoMapper;
using ParsingProject.BLL.Interfaces;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using TL;
using WTelegram;

namespace ParsingProject.BLL.Services;

public class ChannelService : BaseService, IChannelService
{
    public ChannelService(ParsingProjectContext context, IMapper mapper) :
        base(context, mapper) { }

    public List<DAL.Entities.Channel> GetChannelsToParse()
    {
        return _context.Channels.Where(c => !c.IsDeleted).ToList();
    }

    public async Task<bool> SaveChannel(string channelLink, Client client)
    {
        var link = channelLink;

        var chatUsername = link.Split('/')[^1];

        var resolved = await client.Contacts_ResolveUsername(chatUsername);
        if (resolved.Chat is not Channel channel)
            return false;

        await RandomDelay.Wait(2000, 4000);
        
        long chatId = resolved.Chat.ID;
        
        var allChats = await client.Messages_GetAllChats();
        
        await RandomDelay.Wait(2000, 4000);
        
        if (!allChats.chats.TryGetValue(chatId, out _))
        {
            await client.Channels_JoinChannel(channel);
        }

        var ch = _context.Channels.FirstOrDefault(c => c.TelegramId == chatId);
        if (ch is null)
        {
            var channelDbModel = _mapper.Map<DAL.Entities.Channel>(ch);
            _context.Channels.Add(channelDbModel);
            await _context.SaveChangesAsync();
        }
        else
        {
            ch.IsDeleted = false;
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<bool> DeleteChannel(long id)
    {
        var channel = await _context.Channels.FindAsync(id);
        if (channel is null)
            return false;

        channel.IsDeleted = true;
        await _context.SaveChangesAsync();

        return true;
    }
}