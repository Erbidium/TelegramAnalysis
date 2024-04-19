namespace ParsingProject;

public class ParsingUpdateHostedService : BackgroundService
{
    private readonly ILogger<ParsingUpdateHostedService> _logger;
    private readonly IServiceScopeFactory _factory;
    private readonly TimeSpan _period = TimeSpan.FromSeconds(5); //.FromHours(1);
    private int _executionCount;
    public bool IsEnabled { get; set; } = true;
    
    public ParsingUpdateHostedService(
        ILogger<ParsingUpdateHostedService> logger,
        IServiceScopeFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_period);
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                if (IsEnabled)
                {
                    await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                    var parsingService = asyncScope.ServiceProvider.GetRequiredService<IParsingService>();
                    await parsingService.UpdateChannelsDataAsync();
                    
                    _executionCount++;
                    _logger.LogInformation(
                        $"Executed PeriodicHostedService - Count: {_executionCount}");
                }
                else
                {
                    _logger.LogInformation(
                        "Skipped PeriodicHostedService");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    $"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
            }
        }
    }
}