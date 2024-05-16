namespace ParsingProject.BLL;

public static class RandomDelay
{
    public static void Wait(int minMilliSeconds = 60000, int maxMilliSeconds = 90000)
    {
        Random random = new Random();
        var mseconds = random.Next(minMilliSeconds, maxMilliSeconds);   
        Thread.Sleep(mseconds);
    }
}