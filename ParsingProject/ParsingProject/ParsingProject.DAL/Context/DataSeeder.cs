using ParsingProject.DAL.Entities;

namespace ParsingProject.DAL.Context;

public class DataSeeder
{
    private readonly ParsingProjectContext parsingProjectContext;

    public DataSeeder(ParsingProjectContext parsingProjectContext)
    {
        this.parsingProjectContext = parsingProjectContext;
    }

    public void Seed()
    {
        if(!parsingProjectContext.Reactions.Any())
        {
            var reactions = Reactions.ReactionsMap.Keys.Select(r => new Reaction
            {
                Emoticon = r
            }).ToArray();
            
            parsingProjectContext.Reactions.AddRange(reactions);
            parsingProjectContext.SaveChanges();
        }
    }
}