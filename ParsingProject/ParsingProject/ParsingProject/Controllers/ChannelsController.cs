using Microsoft.AspNetCore.Mvc;
using ParsingProject.BLL;
using ParsingProject.BLL.Services;
using ParsingProject.DAL.Context;
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
    
    [HttpGet("channelsToParse")]
    public ActionResult<List<DAL.Entities.Channel>> GetChannelsToParse()
    {
        return _dataContext.Channels.ToList();
    }
    
    [HttpPost("saveChannel")]
    public async Task<IActionResult> SaveChannel(string link)
    {
        if (!link.StartsWith("https://t.me/"))
            return BadRequest();

        var chatUsername = link.Split('/')[^1];
        var client = WT.Client;
        
        var resolved = await client.Contacts_ResolveUsername(chatUsername);
        if (resolved.Chat is not Channel channel)
            return BadRequest();

        RandomDelay.Wait(2000, 4000);
        
        long chatId = resolved.Chat.ID;
        
        var allChats = await client.Messages_GetAllChats();
        
        RandomDelay.Wait(2000, 4000);
        
        if (!allChats.chats.TryGetValue(chatId, out _))
        {
            await client.Channels_JoinChannel(channel);
        }

        if (_dataContext.Channels.FirstOrDefault(c => c.TelegramId == chatId) is null)
        {
            await _repository.SaveChannelAsync(channel);
        }

        return Ok();
    }
}