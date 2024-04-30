using Microsoft.AspNetCore.Mvc;
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

    public ParsingController(
        IChannelParsingService channelParsingService,
        ParsingUpdateHostedService parsingUpdateHostedService,
        ILogger<ParsingController> logger,
        WTelegramService wt)
    {
        _channelParsingService = channelParsingService;
        _parsingUpdateHostedService = parsingUpdateHostedService;
        _logger = logger;
        WT = wt;
    }

    [HttpGet(Name = "ParseChannels")]
    public async Task<IActionResult> Get()
    {
        await _channelParsingService.ParseChannelsDataAsync(WT);

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
}