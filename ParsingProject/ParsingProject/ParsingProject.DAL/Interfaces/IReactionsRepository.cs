using TL;

namespace ParsingProject.DAL.Interfaces;

public interface IReactionsRepository
{
    Task SavePostReactionsAsync(Message message, long postId);

    void SaveCommentReaction(ReactionCount reactionCount, long commentId);

    void SavePostReaction(ReactionCount reactionCount, long postId);
}