using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ParsingProject.BackgroundServices;
using ParsingProject.BLL.Entities;
using ParsingProject.BLL.Interfaces;
using ParsingProject.DTO;

namespace ParsingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ParsingController : ControllerBase
{
    private readonly WTelegramService _wt;
    private readonly IChannelParsingService _channelParsingService;
    private readonly ParsingUpdateHostedService _parsingUpdateHostedService;
    private readonly IValidator<ChannelsParsingDto> _channelsParsingDtoValidator;
    private readonly IStatisticsService _statisticsService;

    public ParsingController(
        IChannelParsingService channelParsingService,
        ParsingUpdateHostedService parsingUpdateHostedService,
        WTelegramService wt,
        IValidator<ChannelsParsingDto> channelsParsingDtoValidator,
        IStatisticsService statisticsService)
    {
        _channelParsingService = channelParsingService;
        _parsingUpdateHostedService = parsingUpdateHostedService;
        _wt = wt;
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
        
        if (_wt.User == null) throw new Exception("Complete the login first");

        await _channelParsingService.ParseChannelsDataAsync(_wt.Client, channelsParsing.ParsingDate, cancellationToken);

        return Ok();
    }
    
    [HttpGet(Name = "Background")]
    public bool GetBackgroundServiceState()
    {
        return _parsingUpdateHostedService.IsEnabled;
    }
    
    [HttpPatch(Name = "Background")]
    public void SetBackgroundServiceState(PeriodicHostedServiceState state)
    {
        _parsingUpdateHostedService.IsEnabled = state.IsEnabled;
    }

    [HttpGet("parsing-statistics")]
    public ActionResult<ParsingStatisticsModel> GetParsingStatistics()
    {
        return _statisticsService.GetParsingStatistics();
    }
}