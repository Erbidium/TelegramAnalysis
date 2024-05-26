using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParsingProject.BLL.Entities;
using ParsingProject.DAL.Context;

namespace ParsingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ParsingController : ControllerBase
{
    private readonly WTelegramService WT;
    
    private readonly ILogger<ParsingController> _logger;

    private readonly IChannelParsingService _channelParsingService;
    private readonly ParsingUpdateHostedService _parsingUpdateHostedService;

    private readonly ParsingProjectContext _dataContext;

    public ParsingController(
        IChannelParsingService channelParsingService,
        ParsingUpdateHostedService parsingUpdateHostedService,
        ILogger<ParsingController> logger,
        WTelegramService wt,
        ParsingProjectContext dataContext)
    {
        _channelParsingService = channelParsingService;
        _parsingUpdateHostedService = parsingUpdateHostedService;
        _logger = logger;
        WT = wt;
        _dataContext = dataContext;
    }

    [HttpPost]
    public async Task<IActionResult> ParseChannels(CancellationToken cancellationToken)
    {
        await _channelParsingService.ParseChannelsDataAsync(WT, cancellationToken);

        return Ok();
    }
    
    /*[HttpGet(Name = "Background")]
    public bool GetBackgroundServiceState()
    {
        return _parsingUpdateHostedService.IsEnabled;
    }
    
    //[HttpPatch(Name = "Background")]
    public void SetBackgroundServiceState(PeriodicHostedServiceState state)
    {
        _parsingUpdateHostedService.IsEnabled = state.IsEnabled;
    }*/

    [HttpGet("parsing-statistics")]
    public ActionResult<ParsingStatisticsModel> GetParsingStatistics()
    {
        return new ParsingStatisticsModel
        {
            ChannelsStatistics = _dataContext.Channels
                .Where(c => !c.IsDeleted)
                .ToList()
                .Select(c =>
                {
                    var channelPosts = _dataContext.Posts.Where(p => p.ChannelId == c.Id);
                    
                    var firstChannelParsedPost = channelPosts.OrderBy(p => p.CreatedAt).FirstOrDefault();
                    var lastChannelParsedPost = channelPosts.OrderByDescending(p => p.CreatedAt).FirstOrDefault();

                    int postsCount = channelPosts.Count();
                    int parsedReactionsCount = _dataContext.PostReactions
                        .Include(r => r.Post)
                        .Count(r => r.Post.ChannelId == c.Id);
                    
                    int parsedCommentsCount = _dataContext.Comments
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
            ).ToList()
        };
    }
}