using AutoMapper;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using TL;
using WTelegram;

namespace ParsingProject.BLL.Services;

public class PostService : BaseService
{
    private DBRepository _dbRepository;

    public PostService(DBRepository dbRepository, ParsingProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _dbRepository = dbRepository;
    }
    
    public async Task SavePostDataAsync(Message message, Channel channel, long channelId, Client client)
    {
        var postId = await _dbRepository.SavePostAsync(message, channelId);
        /*
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

                    var commentId = await _dbRepository.SaveCommentAsync(replyMessage, postId);
                    
                    foreach (var reactionCount in replyMessage.reactions.results)
                    {
                        _dbRepository.SaveCommentReaction(reactionCount, commentId);
                    }
                
                    await _context.SaveChangesAsync();
                }
            
                RandomDelay.Wait();

                if (replies.Count == 0)
                {
                    break;
                }
            }
        }
        */
        
        if (message.reactions is not null)
        {
            await _dbRepository.SavePostReactionsAsync(message, postId);
        }
    }
}