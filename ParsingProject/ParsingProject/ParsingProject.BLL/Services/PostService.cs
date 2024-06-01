using AutoMapper;
using ParsingProject.BLL.Interfaces;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using ParsingProject.DAL.Entities;
using ParsingProject.DAL.Repositories;
using TL;
using WTelegram;
using Channel = TL.Channel;

namespace ParsingProject.BLL.Services;

public class PostService : BaseService, IPostService
{
    private ReactionsRepository _reactionsRepository;

    public PostService(ReactionsRepository reactionsRepository, ParsingProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _reactionsRepository = reactionsRepository;
    }
    
    public async Task SavePostDataAsync(Message message, Channel channel, long channelId, Client client)
    {
        var postDbModel = _mapper.Map<Post>(message);
        _context.Posts.Add(postDbModel);
        await _context.SaveChangesAsync();
        var postId = postDbModel.Id;

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
            await _reactionsRepository.SavePostReactionsAsync(message, postId);
        }
    }
}