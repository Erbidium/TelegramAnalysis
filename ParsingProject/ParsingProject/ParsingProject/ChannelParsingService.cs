using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParsingProject.BLL.Services;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using TL;
using WTelegram;

namespace ParsingProject;

public class ChannelParsingService : BaseService, IChannelParsingService
{
    private TimeSpan _updateDeltaTime = TimeSpan.FromDays(3);

    private PostService _postService;
    private CommentService _commentService;

    private DBRepository _dbRepository;

    public ChannelParsingService
    (
        PostService postService,
        CommentService commentService,
        DBRepository dbRepository,
        ParsingProjectContext context,
        IMapper mapper
    ) :
        base(context, mapper)
    {
        _postService = postService;
        _commentService = commentService;
        _dbRepository = dbRepository;
    }

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

            if (chat is not Channel channel)
                continue;
            
            RandomDelay.Wait();

            await SaveChannelDataAsync(channel, client);
        }
    }
    
    public async Task UpdateChannelsDataAsync(WTelegramService wt)
    {
        /*
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
        */
    }

    private async Task SaveChannelDataAsync(Channel channel, Client client)
    {
        var channelId = await _dbRepository.SaveChannelAsync(channel);

        int limit = 100;
        for (int offset = 0; ; offset += limit)
        {
            var allMessages = await client.Messages_GetHistory(channel, add_offset: offset, limit: limit);

            foreach (var m in allMessages.Messages)
            {
                if (m is not Message message || string.IsNullOrWhiteSpace(message.message))
                    continue;

                await _postService.SavePostDataAsync(message, channel, channelId, client);
            }
            
            RandomDelay.Wait();

            if (allMessages.Count == 0 || allMessages.Messages[0].Date < new DateTime(2024, 1, 1))
            {
                break;
            }
        }
    }

    private async Task UpdateChannelDataAsync(Channel channel, Client client)
    {
        var storedChannel = await _context.Channels.FirstOrDefaultAsync(c => c.TelegramId == channel.ID);

        if (storedChannel is null)
        {
            await SaveChannelDataAsync(channel, client);
            return;
        }
        
        int limit = 1;
        for (int offset = 0;; offset += limit)
        {
            var allMessages = await client.Messages_GetHistory(channel, limit: limit, add_offset: offset);

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
                    await _postService.SavePostDataAsync(message, channel, storedChannel.Id, client);
                    continue;
                }

                if (storedMessage.Hash != message.message.GetHashCode())
                {
                    await _dbRepository.SavePostAsync(message, storedChannel.Id);
                }

                if (message.replies is not null)
                {
                    int repliesLimit = 1;
                    for (int repliesOffset = 0;; repliesOffset += repliesLimit)
                    {
                        var replies = await client.Messages_GetReplies(channel, message.ID, limit: repliesLimit, add_offset: repliesOffset);

                        foreach (var reply in replies.Messages)
                        {
                            if (reply is not Message replyMessage)
                                continue;

                            var storedReply = await _context.Comments
                                .Where(c => c.TelegramId == replyMessage.ID)
                                .OrderByDescending(p => p.ParsedAt)
                                .FirstOrDefaultAsync();

                            if (storedReply is null ||
                                storedReply.Hash != replyMessage.message.GetHashCode())
                            {
                                await _commentService.SaveCommentDataAsync(replyMessage, storedMessage.Id);
                                continue;
                            }

                            if (replyMessage.reactions is not null)
                            {
                                foreach (var reactionCount in replyMessage.reactions.results)
                                {
                                    string emoticon = (reactionCount.reaction as dynamic).emoticon;

                                    var storedReaction = await _context.CommentReactions.Where(r =>
                                            r.CommentId == storedReply.Id &&
                                            r.Emoticon == emoticon)
                                        .OrderByDescending(r => r.ParsedAt)
                                        .FirstOrDefaultAsync();

                                    if (storedReaction == null || storedReaction.Count != reactionCount.count)
                                    {
                                        _dbRepository.SaveCommentReaction(reactionCount, storedReply.Id);
                                    }
                                }
                            }

                            await _context.SaveChangesAsync();
                        }
                        
                        if (replies.Count == 0 || replies.Messages[0].Date < DateTime.Now - _updateDeltaTime)
                        {
                            break;
                        }
                    }
                }

                if (message.reactions is not null)
                {
                    foreach (var reactionCount in message.reactions.results)
                    {
                        string emoticon = (reactionCount.reaction as dynamic).emoticon;

                        var storedReaction = await _context.PostReactions
                            .Include(r => r.Reaction)
                            .Where(r => r.Reaction != null && r.PostId == storedMessage.Id && r.Reaction.Emoticon == emoticon)
                            .OrderByDescending(r => r.ParsedAt)
                            .FirstOrDefaultAsync();

                        if (storedReaction == null || storedReaction.Count != reactionCount.count)
                        {
                            _dbRepository.SavePostReaction(reactionCount, storedMessage.Id);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }
            
            if (allMessages.Count == 0 || allMessages.Messages[0].Date < DateTime.Now - _updateDeltaTime)
            {
                break;
            }
        }
    }
}