using AutoMapper;
using ParsingProject.BLL.Interfaces;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;
using ParsingProject.DAL.Entities;

namespace ParsingProject.BLL.Services;

public class ParsingService : BaseService, IParsingService
{
    public ParsingService(ParsingProjectContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task ParseChannelsDataAsync()
    {
        Console.WriteLine("Parsing data");
        _context.Channels.Add(new Channel
        {
            Name = "Hello, test channel"
        });

        await _context.SaveChangesAsync();
    }

    public async Task UpdateChannelsDataAsync()
    {
        Console.WriteLine("Updating channels data");
        
        _context.Channels.Add(new Channel
        {
            Name = "Updated channel data"
        });

        await _context.SaveChangesAsync();
    }
}