using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParsingProject.BLL.Entities;
using ParsingProject.BLL.Interfaces;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using ParsingProject.DAL.Repositories;

namespace ParsingProject.BLL.Services;

public class StatisticsService : BaseService, IStatisticsService
{
    public StatisticsService(ParsingProjectContext context, IMapper mapper) : base(context, mapper) { }

    public ParsingStatisticsModel GetParsingStatistics()
    {
        return new ParsingStatisticsModel
        {
            ChannelsStatistics = _context.Channels
                .Include(c => c.Posts)
                .Where(c => !c.IsDeleted)
                .ToList()
                .Select(c =>
                    {
                        var firstChannelParsedPost = c.Posts.MinBy(p => p.CreatedAt);
                        var lastChannelParsedPost = c.Posts.MaxBy(p => p.CreatedAt);

                        int postsCount = c.Posts.Count;
                        int parsedReactionsCount = _context.PostReactions
                            .Include(r => r.Post)
                            .Count(r => r.Post.ChannelId == c.Id);
                    
                        int parsedCommentsCount = _context.Comments
                            .Include(comment => comment.Post)
                            .Count(r => r.Post.ChannelId == c.Id);
                    
                        var channelStatisticsModel = new ChannelStatisticsModel
                        {
                            Channel = _mapper.Map<ChannelModel>(c),
                            StartDate = firstChannelParsedPost?.CreatedAt,
                            EndDate = lastChannelParsedPost?.CreatedAt,
                            ParsedPostsCount = postsCount,
                            ParsedReactionsCount = parsedReactionsCount,
                            ParsedCommentsCount = parsedCommentsCount
                        };

                        channelStatisticsModel.Channel.Posts = _mapper.Map<List<PostModel>>(c.Posts.ToList());

                        return channelStatisticsModel;
                    }
                )
                .ToList()
        };
    }
}