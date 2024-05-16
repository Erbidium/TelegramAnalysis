namespace ParsingProject.BLL;

public static class RandomDelay
{
    public static async Task Wait(int minMilliSeconds = 60000, int maxMilliSeconds = 90000)
    {
        Random random = new Random();
        var mseconds = random.Next(minMilliSeconds, maxMilliSeconds);
        await Task.Delay(mseconds);
    }
}