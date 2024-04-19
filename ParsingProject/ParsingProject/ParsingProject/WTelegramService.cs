using TL;

namespace ParsingProject;

public sealed class WTelegramService : BackgroundService
{
    public readonly WTelegram.Client Client;
    public User User => Client.User;
    public string ConfigNeeded = "connecting";

    private readonly IConfiguration _config;

    public WTelegramService(IConfiguration config, ILogger<WTelegramService> logger)
    {
        _config = config;
        WTelegram.Helpers.Log = (lvl, msg) => logger.Log((LogLevel)lvl, msg);
        Client = new WTelegram.Client(what => _config[what]);
    }

    public override void Dispose()
    {
        Client.Dispose();
        base.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ConfigNeeded = await DoLogin(_config["phone_number"]!);
    }

    public async Task<string> DoLogin(string loginInfo)
    {
        return ConfigNeeded = await Client.Login(loginInfo);
    }
}