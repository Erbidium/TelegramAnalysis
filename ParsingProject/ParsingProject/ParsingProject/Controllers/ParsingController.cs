using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParsingProject.BLL.Entities;
using ParsingProject.BLL.Services;
using ParsingProject.DAL.Context;
using ParsingProject.DTO;

namespace ParsingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ParsingController : ControllerBase
{
    private readonly WTelegramService WT;
    
    private readonly ILogger<ParsingController> _logger;

    private readonly IChannelParsingService _channelParsingService;
    private readonly ParsingUpdateHostedService _parsingUpdateHostedService;
    private readonly IValidator<ChannelsParsingDto> _channelsParsingDtoValidator;
    private StatisticsService _statisticsService;

    private readonly ParsingProjectContext _dataContext;

    public ParsingController(
        IChannelParsingService channelParsingService,
        ParsingUpdateHostedService parsingUpdateHostedService,
        ILogger<ParsingController> logger,
        WTelegramService wt,
        ParsingProjectContext dataContext,
        IValidator<ChannelsParsingDto> channelsParsingDtoValidator,
        StatisticsService statisticsService)
    {
        _channelParsingService = channelParsingService;
        _parsingUpdateHostedService = parsingUpdateHostedService;
        _logger = logger;
        WT = wt;
        _dataContext = dataContext;
        _channelsParsingDtoValidator = channelsParsingDtoValidator;
        _statisticsService = statisticsService;
    }

    [HttpPost]
    public async Task<IActionResult> ParseChannels([FromBody]ChannelsParsingDto channelsParsing, CancellationToken cancellationToken)
    {
        var validationResult = await _channelsParsingDtoValidator.ValidateAsync(channelsParsing);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        await _channelParsingService.ParseChannelsDataAsync(WT, channelsParsing.ParsingDate, cancellationToken);

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
        return _statisticsService.GetParsingStatistics();
    }
}