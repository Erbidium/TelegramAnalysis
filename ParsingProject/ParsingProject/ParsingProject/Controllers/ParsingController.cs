using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

    private readonly ChannelsConfig _channels;

    public ParsingController(
        IChannelParsingService channelParsingService,
        ParsingUpdateHostedService parsingUpdateHostedService,
        IOptions<ChannelsConfig> options,
        ILogger<ParsingController> logger,
        WTelegramService wt)
    {
        _channelParsingService = channelParsingService;
        _parsingUpdateHostedService = parsingUpdateHostedService;
        _channels = options.Value;
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