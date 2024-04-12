using Microsoft.AspNetCore.Mvc;
using ParsingProject.BLL.Interfaces;

namespace ParsingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ParsingController : ControllerBase
{
    private readonly ILogger<ParsingController> _logger;

    private readonly IParsingService _parsingService;

    public ParsingController(IParsingService parsingService, ILogger<ParsingController> logger)
    {
        _parsingService = parsingService;
        _logger = logger;
    }

    [HttpGet(Name = "ParseChannels")]
    public IActionResult Get()
    {
        // clear DB
        _parsingService.ParseChannelsData();

        return Ok();
    }
}