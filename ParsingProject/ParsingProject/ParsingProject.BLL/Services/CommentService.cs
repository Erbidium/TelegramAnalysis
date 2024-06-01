using AutoMapper;
using ParsingProject.BLL.Interfaces;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using ParsingProject.DAL.Entities;
using ParsingProject.DAL.Interfaces;
using TL;

namespace ParsingProject.BLL.Services;

public class CommentService : BaseService, ICommentService
{
    private IReactionsRepository _reactionsRepository;

    public CommentService(IReactionsRepository reactionsRepository, ParsingProjectContext context, IMapper mapper) :
        base(context, mapper)
    {
        _reactionsRepository = reactionsRepository;
    }
    
    public async Task SaveCommentDataAsync(Message replyMessage, long postId)
    {
        var commentDbModel = _mapper.Map<Comment>(replyMessage);
        _context.Comments.Add(commentDbModel);
        await _context.SaveChangesAsync();
        var commentId = commentDbModel.Id;

        if (replyMessage.reactions is not null)
        {
            foreach (var reactionCount in replyMessage.reactions.results)
            {
                _reactionsRepository.SaveCommentReaction(reactionCount, commentId);
            }
        }

        await _context.SaveChangesAsync();
    }
}