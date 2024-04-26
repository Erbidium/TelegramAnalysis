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
            
            RandomDelay();

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

    private async Task SaveChannelDataAsync(TL.Channel channel, Client client)
    {
        var channelId = await SaveChannelAsync(channel);

        int limit = 1;
        for (int offset = 0; ; offset += limit)
        {
            var allMessages = await client.Messages_GetHistory(channel, add_offset: offset, limit: 1);

            foreach (var m in allMessages.Messages)
            {
                if (m is not Message message || string.IsNullOrWhiteSpace(message.message))
                    continue;

                await SavePostDataAsync(message, channel, channelId, client);
            }
            
            RandomDelay();

            if (allMessages.Count == 0 || allMessages.Messages[0].Date < new DateTime(2024, 1, 1))
            {
                break;
            }
        }
    }

    private async Task SavePostDataAsync(Message message, TL.Channel channel, long channelId, Client client)
    {
        var postId = await SavePostAsync(message, channelId);
        
        if (message.replies is not null)
        {
            int limit = 1;
            for (int offset = 0; ; offset += limit)
            {
                var replies = await client.Messages_GetReplies(channel, message.ID, limit: limit, add_offset: offset);

                foreach (var reply in replies.Messages)
                {
                    if (reply is not Message replyMessage || string.IsNullOrWhiteSpace(replyMessage.message))
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
            
                RandomDelay();

                if (replies.Count == 0)
                {
                    break;
                }
            }
        }
        if (message.reactions is not null)
        {
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
    }
    
    private async Task SaveCommentDataAsync(Message replyMessage, long postId)
    {
        var commentId = await SaveCommentAsync(replyMessage, postId);

        if (replyMessage.reactions is not null)
        {
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
                    await SavePostDataAsync(message, channel, storedChannel.Id, client);
                    continue;
                }

                if (storedMessage.Hash != message.message.GetHashCode())
                {
                    await SavePostAsync(message, storedChannel.Id);
                }

                if (message.replies is not null)
                {
                    int repliesLimit = 1;
                    for (int repliesOffset = 0;; repliesOffset += repliesLimit)
                    {
                        var replies = await client.Messages_GetReplies(channel, message.ID, limit: repliesLimit, add_offset: offset);

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
                                await SaveCommentDataAsync(replyMessage, storedMessage.Id);
                                continue;
                            }

                            if (replyMessage.reactions is not null)
                            {
                                foreach (var reactionCount in replyMessage.reactions.results)
                                {
                                    string reaction =
                                        Reactions.ReactionsMap((reactionCount.reaction as dynamic).emoticon);

                                    var storedReaction = await _context.CommentReactions.Where(r =>
                                            r.CommentId == storedReply.Id &&
                                            r.Reaction == reaction)
                                        .OrderByDescending(r => r.ParsedAt)
                                        .FirstOrDefaultAsync();

                                    if (storedReaction == null || storedReaction.Count != reactionCount.count)
                                    {
                                        _context.CommentReactions.Add(new CommentReaction
                                        {
                                            CommentId = storedReply.Id,
                                            Emoticon = (reactionCount.reaction as dynamic).emoticon,
                                            Reaction = Reactions.ReactionsMap((reactionCount.reaction as dynamic)
                                                .emoticon),
                                            Count = reactionCount.count,
                                            ParsedAt = DateTime.Now
                                        });
                                    }
                                }
                            }

                            await _context.SaveChangesAsync();
                        }
                        
                        if (replies.Count == 0 || replies.Messages[0].Date < DateTime.Now - TimeSpan.FromDays(3))
                        {
                            break;
                        }
                    }
                }

                if (message.reactions is not null)
                {
                    foreach (var reactionCount in message.reactions.results)
                    {
                        string reaction = Reactions.ReactionsMap((reactionCount.reaction as dynamic).emoticon);

                        var storedReaction = await _context.PostReactions.Where(r => r.PostId == storedMessage.Id &&
                                r.Reaction == reaction)
                            .OrderByDescending(r => r.ParsedAt)
                            .FirstOrDefaultAsync();

                        if (storedReaction == null || storedReaction.Count != reactionCount.count)
                        {
                            _context.PostReactions.Add(new PostReaction
                            {
                                PostId = storedMessage.Id,
                                Emoticon = (reactionCount.reaction as dynamic).emoticon,
                                Reaction = Reactions.ReactionsMap((reactionCount.reaction as dynamic).emoticon),
                                Count = reactionCount.count,
                                ParsedAt = DateTime.Now
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }
            
            if (allMessages.Count == 0 || allMessages.Messages[0].Date < DateTime.Now - TimeSpan.FromDays(3))
            {
                break;
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
            CreatedAt = post.Date,
            EditedAt = post.edit_date,
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
            Hash = comment.message.GetHashCode(),
            ViewsCount = comment.views,
            CreatedAt = comment.Date,
            EditedAt = comment.edit_date,
            PostId = messageId,
            ParsedAt = DateTime.Now
        };

        _context.Comments.Add(commentDbModel);
        await _context.SaveChangesAsync();

        return commentDbModel.Id;
    }

    private void RandomDelay()
    {
        // Random delay
        Random random = new Random();
        var mseconds = random.Next(1000, 4000);   
        Thread.Sleep(mseconds);
    }
}