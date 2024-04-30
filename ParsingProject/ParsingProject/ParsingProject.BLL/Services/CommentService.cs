using AutoMapper;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using TL;

namespace ParsingProject.BLL.Services;

public class CommentService : BaseService
{
    private DBRepository _dbRepository;

    public CommentService(DBRepository dbRepository, ParsingProjectContext context, IMapper mapper) :
        base(context, mapper)
    {
        _dbRepository = dbRepository;
    }
    
    public async Task SaveCommentDataAsync(Message replyMessage, long postId)
    {
        var commentId = await _dbRepository.SaveCommentAsync(replyMessage, postId);

        if (replyMessage.reactions is not null)
        {
            foreach (var reactionCount in replyMessage.reactions.results)
            {
                _dbRepository.SaveCommentReaction(reactionCount, commentId);
            }
        }

        await _context.SaveChangesAsync();
    }
}