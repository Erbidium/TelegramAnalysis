using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ParsingProject.BackgroundServices;
using ParsingProject.BLL.Interfaces;
using ParsingProject.DTO;

namespace ParsingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ChannelsController : ControllerBase
{
    private readonly WTelegramService _wt;
    private readonly IValidator<SaveChannelDto> _saveChannelDtoValidator;
    private readonly IChannelService _channelService;
    
    public ChannelsController
    (
        WTelegramService wt,
        IValidator<SaveChannelDto> saveChannelDtoValidator,
        IChannelService channelService
    )
    {
        _wt = wt;
        _saveChannelDtoValidator = saveChannelDtoValidator;
        _channelService = channelService;
    }
    
    [HttpGet("channels-to-parse")]
    public ActionResult<List<DAL.Entities.Channel>> GetChannelsToParse()
    {
        return _channelService.GetChannelsToParse();
    }
    
    [HttpPost("save-channel")]
    public async Task<IActionResult> SaveChannel([FromBody] SaveChannelDto saveChannelDto)
    {
        var validationResult = await _saveChannelDtoValidator.ValidateAsync(saveChannelDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        return await _channelService.SaveChannel(saveChannelDto.ChannelLink, _wt.Client) ? Ok() : BadRequest();
    }
    
    [HttpDelete("delete-channel/{id:long}")]
    public async Task<IActionResult> DeleteChannel(long id)
    {
        return await _channelService.DeleteChannel(id) ? Ok() : BadRequest();
    }
}