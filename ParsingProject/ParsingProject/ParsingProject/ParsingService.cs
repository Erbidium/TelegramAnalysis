using AutoMapper;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using ParsingProject.DAL.Entities;

namespace ParsingProject;

public class ParsingService : BaseService, IParsingService
{
    public ParsingService(ParsingProjectContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task ParseChannelsDataAsync(WTelegramService wt)
    {
        var dataParser = new DataParser();
        await dataParser.ParseData(wt, _context);
    }

    public async Task UpdateChannelsDataAsync()
    {
        Console.WriteLine("Updating channels data");
        
        _context.Channels.Add(new Channel
        {
            MainUsername = "TestUser",
            Title = "Updated channel data"
        });

        await _context.SaveChangesAsync();
    }
}