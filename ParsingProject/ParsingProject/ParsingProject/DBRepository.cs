using ParsingProject.DAL.Context;
using ParsingProject.DAL.Entities;
using TL;
using Channel = ParsingProject.DAL.Entities.Channel;

namespace ParsingProject;

public class DBRepository
{
    private ParsingProjectContext _context;

    public DBRepository(ParsingProjectContext context)
    {
        _context = context;
    }

    public async Task<long> SaveChannelAsync(TL.Channel chat)
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
    
    public async Task<long> SavePostAsync(Message post, long channelId)
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
    
    public async Task<long> SaveCommentAsync(Message comment, long messageId)
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
}