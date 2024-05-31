using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParsingProject.BLL.Entities;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;

namespace ParsingProject.BLL.Services;

public class StatisticsService : BaseService
{
    private DBRepository _dbRepository;
    
    public StatisticsService(DBRepository dbRepository, ParsingProjectContext context, IMapper mapper) : base(context, mapper)
    {
        _dbRepository = dbRepository;
    }

    public ParsingStatisticsModel GetParsingStatistics()
    {
        return new ParsingStatisticsModel
        {
            ChannelsStatistics = _context.Channels
                .Where(c => !c.IsDeleted)
                .ToList()
                .Select(c =>
                    {
                        var channelPosts = _context.Posts.Where(p => p.ChannelId == c.Id);
                    
                        var firstChannelParsedPost = channelPosts.OrderBy(p => p.CreatedAt).FirstOrDefault();
                        var lastChannelParsedPost = channelPosts.OrderByDescending(p => p.CreatedAt).FirstOrDefault();

                        int postsCount = channelPosts.Count();
                        int parsedReactionsCount = _context.PostReactions
                            .Include(r => r.Post)
                            .Count(r => r.Post.ChannelId == c.Id);
                    
                        int parsedCommentsCount = _context.Comments
                            .Include(comment => comment.Post)
                            .Count(r => r.Post.ChannelId == c.Id);
                    
                        return new ChannelStatisticsModel
                        {
                            Channel = new ChannelModel
                            {
                                Id = c.Id,
                                TelegramId = c.TelegramId,
                                ParticipantsCount = c.ParticipantsCount,
                                MainUsername = c.MainUsername,
                                Title = c.Title
                            },
                            StartDate = firstChannelParsedPost?.CreatedAt,
                            EndDate = lastChannelParsedPost?.CreatedAt,
                            ParsedPostsCount = postsCount,
                            ParsedReactionsCount = parsedReactionsCount,
                            ParsedCommentsCount = parsedCommentsCount
                        };
                    }
                )
                .ToList()
        };
    }
}