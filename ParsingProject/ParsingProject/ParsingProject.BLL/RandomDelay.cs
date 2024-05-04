namespace ParsingProject;

public static class RandomDelay
{
    public static void Wait()
    {
        Random random = new Random();
        var mseconds = random.Next(60000, 90000);   
        Thread.Sleep(mseconds);
    }
}