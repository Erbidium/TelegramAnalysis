using ParsingProject.DAL.Entities;

namespace ParsingProject.DAL.Context;

public class DataSeeder
{
    private readonly ParsingProjectContext _parsingProjectContext;

    public DataSeeder(ParsingProjectContext parsingProjectContext)
    {
        this._parsingProjectContext = parsingProjectContext;
    }

    public void Seed()
    {
        if(!_parsingProjectContext.Reactions.Any())
        {
            var reactions = Reactions.ReactionsMap.Keys.Select(r => new Reaction
            {
                Emoticon = r
            }).ToArray();
            
            _parsingProjectContext.Reactions.AddRange(reactions);
            _parsingProjectContext.SaveChanges();
        }
    }
}