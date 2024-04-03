using Microsoft.AspNetCore.Mvc;

namespace ParsingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ParsingController : ControllerBase
{
    private readonly ILogger<ParsingController> _logger;

    public ParsingController(ILogger<ParsingController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "ParseChannels")]
    public IActionResult Get()
    {
        // clear DB
        // call parsing service

        return Ok();
    }
}