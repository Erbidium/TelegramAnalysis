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
    { }

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
                
                var postId = await SavePostAsync(message, channelId);

                if (message.replies is not null)
                {
                    var replies = await client.Messages_GetReplies(chat, m.ID);

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
                                Reaction = ReactionsMap((reactionCount.reaction as dynamic).emoticon),
                                Count = reactionCount.count,
                                ParsedAt = DateTime.Now
                            });
                        }
                    }
                }

                foreach (var reactionCount in message.reactions.results)
                {
                    _context.PostReactions.Add(new PostReaction
                    {
                        PostId = postId,
                        Emoticon = (reactionCount.reaction as dynamic).emoticon,
                        Reaction = ReactionsMap((reactionCount.reaction as dynamic).emoticon),
                        Count = reactionCount.count,
                        ParsedAt = DateTime.Now
                    });
                }

                await _context.SaveChangesAsync();
            }
        }
    }

    private static string ReactionsMap(string reaction)
    {
        return reaction switch
        {
            "👍" => "thumbs up",
            "👎" => "thumbs down",
            "❤" => "red heart",
            "🔥" => "fire",
            "🥰" => "smiling face",
            "👏" => "clapping hands",
            "😁" => "grinning face",
            "🤔" => "thinking face",
            "🤯" => "exploding head",
            "😱" => "scream",
            "🤬" => "face with symbols",
            "😢" => "crying face",
            "🎉" => "party popper",
            "🤩" => "star-struck",
            "🤮" => "face vomiting",
            "💩" => "pile of poo",
            "🙏" => "folded hands",
            "👌" => "ok hand",
            "🕊" => "dove of peace",
            "🤡" => "clown face",
            "🥱" => "yawning face",
            "🥴" => "woozy face",
            "😍" => "smiling face with heart-shaped eyes",
            "🐳" => "spouting whale",
            "❤‍🔥" => "heart on fire",
            "🌚" => "new moon face",
            "🌭" => "hot dog",
            "💯" => "hundred points",
            "🤣" => "rolling on the floor laughing",
            "⚡" => "high voltage",
            "🍌" => "banana",
            "🏆" => "trophy",
            "💔" => "broken heart",
            "🤨" => "face with raised eyebrow",
            "😐" => "neutral face",
            "🍓" => "strawberry",
            "🍾" => "bottle with popping cork",
            "💋" => "kiss mark",
            "🖕" => "middle finger",
            "😈" => "smiling face with horns",
            "😴" => "sleeping face",
            "😭" => "loudly crying face",
            "🤓" => "nerd face",
            "👻" => "ghost",
            "👨‍💻" => "man technologist",
            "👀" => "eyes",
            "🎃" => "jack-o-lantern",
            "🙈" => "see-no-evil",
            "😇" => "smiling face with halo",
            "😨" => "fearful face",
            "🤝" => "",
            "✍" => "",
            "🤗" => "",
            "🫡" => "",
            "🎅" => "",
            "🎄" => "",
            "☃" => "",
            "💅" => "",
            "🤪" => "",
            "🗿" => "",
            "🆒" => "",
            "💘" => "",
            "🙉" => "",
            "🦄" => "",
            "😘" => "",
            "💊" => "",
            "🙊" => "",
            "😎" => "",
            "👾" => "",
            "🤷‍♂" => "man shrugging",
            "🤷" => "peson shrugging",
            "🤷‍♀" => "woman shrugging",
            "😡" => "enraged face",
            _ => "other"
                
        };
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