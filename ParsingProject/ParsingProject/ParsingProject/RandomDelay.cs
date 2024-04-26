namespace ParsingProject;

public static class RandomDelay
{
    public static void Wait()
    {
        Random random = new Random();
        var mseconds = random.Next(1000, 4000);   
        Thread.Sleep(mseconds);
    }
}