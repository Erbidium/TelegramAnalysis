using Microsoft.AspNetCore.Mvc;
using ParsingProject.BLL.Interfaces;
using ParsingProject.DTO;

namespace ParsingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ParsingController : ControllerBase
{
    private readonly ILogger<ParsingController> _logger;

    private readonly IParsingService _parsingService;
    private readonly ParsingUpdateHostedService _parsingUpdateHostedService;

    public ParsingController(
        IParsingService parsingService,
        ParsingUpdateHostedService parsingUpdateHostedService,
        ILogger<ParsingController> logger)
    {
        _parsingService = parsingService;
        _parsingUpdateHostedService = parsingUpdateHostedService;
        _logger = logger;
    }

    [HttpGet(Name = "ParseChannels")]
    public async Task<IActionResult> Get()
    {
        await _parsingService.ParseChannelsDataAsync();

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