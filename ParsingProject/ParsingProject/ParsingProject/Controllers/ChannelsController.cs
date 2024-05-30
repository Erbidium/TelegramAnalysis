using Microsoft.AspNetCore.Mvc;
using ParsingProject.BLL;
using ParsingProject.BLL.Services;
using ParsingProject.DAL.Context;
using ParsingProject.DTO;
using TL;

namespace ParsingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ChannelsController : ControllerBase
{
    private readonly WTelegramService WT;

    private readonly ParsingProjectContext _dataContext;

    private readonly DBRepository _repository;
    
    public ChannelsController
    (
        WTelegramService wt,
        ParsingProjectContext dataContext,
        DBRepository dbRepository
    )
    {
        WT = wt;
        _dataContext = dataContext;
        _repository = dbRepository;
    }
    
    [HttpGet("channels-to-parse")]
    public ActionResult<List<DAL.Entities.Channel>> GetChannelsToParse()
    {
        return _dataContext.Channels.Where(c => !c.IsDeleted).ToList();
    }
    
    [HttpPost("save-channel")]
    public async Task<IActionResult> SaveChannel([FromBody] SaveChannelDto saveChannelDto)
    {
        var link = saveChannelDto.ChannelLink;
        if (!link.StartsWith("https://t.me/"))
            return BadRequest();

        var chatUsername = link.Split('/')[^1];
        var client = WT.Client;
        
        var resolved = await client.Contacts_ResolveUsername(chatUsername);
        if (resolved.Chat is not Channel channel)
            return BadRequest();

        await RandomDelay.Wait(2000, 4000);
        
        long chatId = resolved.Chat.ID;
        
        var allChats = await client.Messages_GetAllChats();
        
        await RandomDelay.Wait(2000, 4000);
        
        if (!allChats.chats.TryGetValue(chatId, out _))
        {
            await client.Channels_JoinChannel(channel);
        }

        var ch = _dataContext.Channels.FirstOrDefault(c => c.TelegramId == chatId);
        if (ch is null)
        {
            await _repository.SaveChannelAsync(channel);
        }
        else
        {
            ch.IsDeleted = false;
            await _dataContext.SaveChangesAsync();
        }

        return Ok();
    }
    
    [HttpDelete("delete-channel/{id:long}")]
    public async Task<IActionResult> DeleteChannel(long id)
    {
        var channel = await _dataContext.Channels.FindAsync(id);
        if (channel is null)
            return BadRequest();

        channel.IsDeleted = true;
        await _dataContext.SaveChangesAsync();
        
        return Ok();
    }
}