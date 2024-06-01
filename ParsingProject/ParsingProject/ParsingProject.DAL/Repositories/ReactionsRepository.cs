using ParsingProject.DAL.Context;
using ParsingProject.DAL.Entities;
using ParsingProject.DAL.Interfaces;
using TL;

namespace ParsingProject.DAL.Repositories;

public class ReactionsRepository : IReactionsRepository
{
    private readonly ParsingProjectContext _context;

    public ReactionsRepository(ParsingProjectContext context)
    {
        _context = context;
    }

    public async Task SavePostReactionsAsync(Message message, long postId)
    {
        foreach (var reactionCount in message.reactions.results)
        {
            SavePostReaction(reactionCount, postId);
        }

        await _context.SaveChangesAsync();
    }

    private void SavePostReaction(ReactionCount reactionCount, long postId)
    {
        string postEmoticon = (reactionCount.reaction as dynamic).emoticon;
        var reactionId = _context.Reactions.FirstOrDefault(r => r.Emoticon == postEmoticon)?.Id;
        
        _context.PostReactions.Add(new PostReaction
        {
            PostId = postId,
            ReactionId = reactionId,
            Count = reactionCount.count,
            ParsedAt = DateTime.Now
        });
    }
    
    public void SaveCommentReaction(ReactionCount reactionCount, long commentId)
    {
        string commentEmoticon = (reactionCount.reaction as dynamic).emoticon;
        var reactionId = _context.Reactions.FirstOrDefault(r => r.Emoticon == commentEmoticon)?.Id;

        _context.CommentReactions.Add(new CommentReaction
        {
            CommentId = commentId,
            ReactionId = reactionId,
            Count = reactionCount.count,
            ParsedAt = DateTime.Now
        });
    }
}