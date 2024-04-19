using Microsoft.AspNetCore.Mvc;

namespace ParsingProject.Controllers;

[ApiController]
[Route("[controller]")]
public class UserBotController : ControllerBase
{
    private readonly WTelegramService WT;
    public UserBotController(WTelegramService wt) => WT = wt;

    [HttpGet("status")]
    public ContentResult Status()
    {
        switch (WT.ConfigNeeded)
        {
            case "connecting": return Content("<meta http-equiv=\"refresh\" content=\"1\">WTelegram is connecting...", "text/html");
            case null: return Content($@"Connected as {WT.User}<br/><a href=""chats"">Get all chats</a>", "text/html");
            default: return Content($@"Enter {WT.ConfigNeeded}: <form action=""config""><input name=""value"" autofocus/></form>", "text/html");
        }
    }

    [HttpGet("config")]
    public async Task<ActionResult> Config(string value)
    {
        await WT.DoLogin(value);
        return Redirect("status");
    }

    [HttpGet("chats")]
    public async Task<object> Chats()
    {
        if (WT.User == null) throw new Exception("Complete the login first");
        var chats = await WT.Client.Messages_GetAllChats();
        return chats.chats;
    }
}