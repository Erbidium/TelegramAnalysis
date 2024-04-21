using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParsingProject.BLL.Services;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using ParsingProject.DAL.Entities;
using TL;
using WTelegram;
using Channel = ParsingProject.DAL.Entities.Channel;

namespace ParsingProject;

public class ParsingService : BaseService, IParsingService
{
    public ParsingService(ParsingProjectContext context, IMapper mapper) : base(context, mapper)
    { }

    public async Task ParseChannelsDataAsync(WTelegramService wt)
    {
        var client = wt.Client;
        
        //if (wt.User == null) throw new Exception("Complete the login first");
        
        var myself = await client.LoginUserIfNeeded();
        Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");

        var chats = await client.Messages_GetAllChats();

        foreach (var (_, chat) in chats.chats)
        {
            if (!chat.IsChannel || !chat.IsActive)
                continue;

            if (chat is not TL.Channel channel)
                continue;

            await SaveChannelDataAsync(channel, client);
        }
    }
    
    public async Task UpdateChannelsDataAsync(WTelegramService wt)
    {
        Console.WriteLine("Updating channels data");
        
        var client = wt.Client;
        
        //if (wt.User == null) throw new Exception("Complete the login first");
        
        var myself = await client.LoginUserIfNeeded();
        Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");

        var chats = await client.Messages_GetAllChats();

        foreach (var (_, chat) in chats.chats)
        {
            if (!chat.IsChannel || !chat.IsActive)
                continue;

            if (chat is not TL.Channel channel)
                continue;

            await UpdateChannelDataAsync(channel, client);
        }
    }

    private async Task SaveChannelDataAsync(TL.Channel channel, Client client)
    {
        var channelId = await SaveChannelAsync(channel);
        var allMessages = await client.Messages_GetHistory(channel);

        foreach (var m in allMessages.Messages)
        {
            if (m is not Message message)
                continue;

            await SavePostDataAsync(message, channel, channelId, client);
        }
    }

    private async Task SavePostDataAsync(Message message, TL.Channel channel, long channelId, Client client)
    {
        var postId = await SavePostAsync(message, channelId);

        if (message.replies is not null)
        {
            var replies = await client.Messages_GetReplies(channel, message.ID);

            foreach (var reply in replies.Messages)
            {
                if (reply is not Message replyMessage)
                    continue;

                var commentId = await SaveCommentAsync(replyMessage, postId);
                    
                foreach (var reactionCount in replyMessage.reactions.results)
                {
                    _context.CommentReactions.Add(new CommentReaction
                    {
                        CommentId = commentId,
                        Emoticon = (reactionCount.reaction as dynamic).emoticon,
                        Reaction = Reactions.ReactionsMap((reactionCount.reaction as dynamic).emoticon),
                        Count = reactionCount.count,
                        ParsedAt = DateTime.Now
                    });
                }
                
                await _context.SaveChangesAsync();
            }
        }

        foreach (var reactionCount in message.reactions.results)
        {
            _context.PostReactions.Add(new PostReaction
            {
                PostId = postId,
                Emoticon = (reactionCount.reaction as dynamic).emoticon,
                Reaction = Reactions.ReactionsMap((reactionCount.reaction as dynamic).emoticon),
                Count = reactionCount.count,
                ParsedAt = DateTime.Now
            });
        }

        await _context.SaveChangesAsync();    
    }

    private async Task UpdateChannelDataAsync(TL.Channel channel, Client client)
    {
        var storedChannel = await _context.Channels.FirstOrDefaultAsync(c => c.TelegramId == channel.ID);

        if (storedChannel is null)
        {
            await SaveChannelDataAsync(channel, client);
            return;
        }
        
        var allMessages = await client.Messages_GetHistory(channel);

        foreach (var m in allMessages.Messages)
        {
            if (m is not Message message)
                continue;
            
            var storedMessage = await _context.Posts
                .Where(p => p.TelegramId == message.ID)
                .OrderByDescending(p => p.ParsedAt)
                .FirstOrDefaultAsync();

            if (storedMessage is null)
            {
                await SavePostDataAsync(message, channel, storedChannel.Id, client);
                continue;
            }

            if (storedMessage.Hash != message.message.GetHashCode() ||
                storedMessage.Text != message.message)
            {
                await SavePostAsync(message, storedChannel.Id);
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
    
    private async Task<long> SavePostAsync(Message post, long channelId)
    {
        var postDbModel = new Post
        {
            TelegramId = post.ID,
            Text = post.message,
            Hash = post.message.GetHashCode(),
            ViewsCount = post.views,
            Date = post.Date,
            EditDate = post.edit_date,
            ParsedAt = DateTime.Now,
            ChannelId = channelId
        };

        _context.Posts.Add(postDbModel);
        await _context.SaveChangesAsync();

        return postDbModel.Id;
    }
    
    private async Task<long> SaveCommentAsync(Message comment, long messageId)
    {
        var commentDbModel = new Comment
        {
            TelegramId = comment.ID,
            Text = comment.message,
            ViewsCount = comment.views,
            Date = comment.Date,
            EditDate = comment.edit_date,
            PostId = messageId,
            ParsedAt = DateTime.Now
        };

        _context.Comments.Add(commentDbModel);
        await _context.SaveChangesAsync();

        return commentDbModel.Id;
    }
}