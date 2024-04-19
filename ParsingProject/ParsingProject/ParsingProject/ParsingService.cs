using AutoMapper;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using ParsingProject.DAL.Entities;
using TL;
using Channel = ParsingProject.DAL.Entities.Channel;

namespace ParsingProject;

public class ParsingService : BaseService, IParsingService
{
    public ParsingService(ParsingProjectContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task ParseChannelsDataAsync(WTelegramService wt)
    {
        var client = wt.Client;
        
        var myself = await client.LoginUserIfNeeded();
        Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");

        var chats = await client.Messages_GetAllChats();

        foreach (var (_, chat) in chats.chats)
        {
            if (!chat.IsChannel || !chat.IsActive)
                continue;

            if (chat is not TL.Channel channel)
                continue;
            
            var channelId = await SaveChannelAsync(channel);

            var allMessages = await client.Messages_GetHistory(channel);

            foreach (var m in allMessages.Messages)
            {
                if (m is not Message message)
                    continue;
                
                await SavePostAsync(message, channelId);

                /*var reactions = await client.Messages_GetMessagesReactions(chat, m.ID);
                var allReactions = (reactions.UpdateList[0] as UpdateMessageReactions)!.reactions;
                
                foreach (var reactionCount in allReactions.results)
                {
                    Console.WriteLine($"{(reactionCount.reaction as dynamic).emoticon}: {reactionCount.count}");
                }

                var result = await client.Messages_GetDiscussionMessage(chat, m.ID);

                var replies = await client.Messages_GetReplies(chat, m.ID);

                foreach (var reply in replies.Messages)
                {
                    Console.WriteLine(reply);
                }
                */
            }
        }
    }

    private async Task<long> SaveChannelAsync(TL.Channel chat)
    {
        var channelDbModel = new Channel
        {
            MainUsername = chat.MainUsername,
            Title = chat.Title,
            TelegramId = chat.ID,
            ParticipantsCount = chat.participants_count
        };

        _context.Channels.Add(channelDbModel);
        await _context.SaveChangesAsync();

        return channelDbModel.Id;
    }
    
    private async Task SavePostAsync(Message post, long channelId)
    {
        var postDbModel = new Post
        {
            TelegramId = post.ID,
            Text = post.message,
            ViewsCount = post.views,
            Date = post.Date,
            EditDate = post.edit_date,
            ChannelId = channelId
        };

        _context.Posts.Add(postDbModel);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateChannelsDataAsync()
    {
        Console.WriteLine("Updating channels data");
        
        _context.Channels.Add(new Channel
        {
            MainUsername = "TestUser",
            Title = "Updated channel data"
        });

        await _context.SaveChangesAsync();
    }
}