using TL;

namespace ParsingProject.BLL.Interfaces;

public interface ICommentService
{
    Task SaveCommentDataAsync(Message replyMessage, long postId);
}