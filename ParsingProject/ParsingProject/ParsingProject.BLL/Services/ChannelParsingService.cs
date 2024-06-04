using System.Globalization;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using ParsingProject.BLL.Entities;
using ParsingProject.BLL.Interfaces;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using ParsingProject.DAL.Entities;
using ParsingProject.DAL.Interfaces;
using TL;
using WTelegram;
using Channel = TL.Channel;

namespace ParsingProject.BLL.Services;

public class ChannelParsingService : BaseService, IChannelParsingService
{
    private TimeSpan _updateDeltaTime = TimeSpan.FromDays(3);

    private IPostService _postService;
    private ICommentService _commentService;
    private IReactionsRepository _reactionsRepository;

    public ChannelParsingService
    (
        IPostService postService,
        ICommentService commentService,
        ParsingProjectContext context,
        IReactionsRepository reactionsRepository,
        IMapper mapper
    ) :
        base(context, mapper)
    {
        _postService = postService;
        _commentService = commentService;
        _reactionsRepository = reactionsRepository;
    }

    public async Task ParseChannelsDataAsync(Client client, DateTime parseDataUntilDate, CancellationToken cancellationToken)
    {
        var messagesCountPerRequest = 100;

        bool allChannelsAreParsed = false;

        //var myself = await client.LoginUserIfNeeded();
        //Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");

        while (!allChannelsAreParsed && !cancellationToken.IsCancellationRequested)
        {
            /*
            allChannelsAreParsed = true;
            
            var channels = _context.Channels.ToList();
        
            await RandomDelay.Wait(1000, 1500);
            var chats = await client.Messages_GetAllChats();
            await RandomDelay.Wait();
            
            foreach (var ch in channels)
            {
                var chat = chats.chats[ch.TelegramId];
                if (chat is not Channel channel)
                    continue;
                
                // find oldest parsed message
                var oldestMessage = await _context.Posts
                    .Where(p => p.ChannelId == ch.Id)
                    .OrderBy(p => p.CreatedAt)
                    .FirstOrDefaultAsync();
                
                if (oldestMessage is null)
                {
                    allChannelsAreParsed = false;
                    await SaveChannelDataAsync(channel, ch.Id, client, 0, messagesCountPerRequest);
                }
                else if (oldestMessage.CreatedAt > parseDataUntilDate)
                {
                    allChannelsAreParsed = false;
                    await SaveChannelDataAsync(channel, ch.Id, client, offset_id: (int)oldestMessage.TelegramId, offset: 1, limit: messagesCountPerRequest);
                }
                else
                {
                    continue;
                }
                
                await RandomDelay.Wait();
            }
            */
        }
        
        Console.WriteLine("Ended parsing");
    }
    
    public async Task UpdateChannelsDataAsync(Client client)
    {
        Console.WriteLine("Updating channels data");
        
        /*
        var myself = await client.LoginUserIfNeeded();
        Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");

        var chats = await client.Messages_GetAllChats();

        foreach (var (_, chat) in chats.chats)
        {
            if (!chat.IsChannel || !chat.IsActive)
                continue;

            if (chat is not Channel channel)
                continue;

            await UpdateChannelDataAsync(channel, client);
        }
        */
    }

    public async Task BuildParsingDataset()
    {
        var postDataset = _context.Posts
            .Include(p => p.Channel)
            .Where(p => !p.Channel.IsDeleted)
            .Select(p => new PostDatasetModel
            {
                Date = p.CreatedAt,
                ChannelName = p.Channel.Title,
                PostText = p.Text
            })
            .ToList();
            
        
        await using var writer = new StreamWriter($"{Directory.GetCurrentDirectory()}\\dataset.csv");
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        
        await csv.WriteRecordsAsync(postDataset);
    }

    private async Task SaveChannelDataAsync(Channel channel, long channelId, Client client, int offset, int limit, int? offset_id = null)
    {
        var messages = await client.Messages_GetHistory(channel, add_offset: offset, limit: limit, offset_id: offset_id ?? 0);

        foreach (var m in messages.Messages)
        {
            if (m is not Message message || string.IsNullOrWhiteSpace(message.message))
                continue;

            var storedMessage = await _context.Posts
                .Where(p => p.TelegramId == message.ID)
                .OrderByDescending(p => p.ParsedAt)
                .FirstOrDefaultAsync();

            if (storedMessage is null)
            {
                await _postService.SavePostDataAsync(message, channel, channelId, client);
            }
        }
    }

    private async Task UpdateChannelDataAsync(Channel channel, Client client)
    {
        /*var storedChannel = await _context.Channels.FirstOrDefaultAsync(c => c.TelegramId == channel.ID);

        if (storedChannel is null)
        {
            await SaveChannelDataAsync(channel, channel.ID, client, 0, 100);
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
                    var postDbModel = _mapper.Map<Post>(message);
                    postDbModel.ChannelId = storedChannel.Id;
                    _context.Posts.Add(postDbModel);
                    await _context.SaveChangesAsync();
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

                                    var storedReaction = await _context
                                        .CommentReactions
                                        .Include(cr => cr.Reaction)
                                        .Where(r =>
                                            r.CommentId == storedReply.Id &&
                                            r.Reaction!.Emoticon == emoticon)
                                        .OrderByDescending(r => r.ParsedAt)
                                        .FirstOrDefaultAsync();

                                    if (storedReaction == null || storedReaction.Count != reactionCount.count)
                                    {
                                        _reactionsRepository.SaveCommentReaction(reactionCount, storedReply.Id);
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
                            _reactionsRepository.SavePostReaction(reactionCount, storedMessage.Id);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }
            
            if (allMessages.Count == 0 || allMessages.Messages[0].Date < DateTime.Now - _updateDeltaTime)
            {
                break;
            }
        }*/
    }
}