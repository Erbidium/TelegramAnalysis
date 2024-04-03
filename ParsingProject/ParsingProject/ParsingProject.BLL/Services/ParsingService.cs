using AutoMapper;
using ParsingProject.BLL.Interfaces;
using ParsingProject.BLL.Services.Abstract;
using ParsingProject.DAL.Context;

namespace ParsingProject.BLL.Services;

public class ParsingService : BaseService, IParsingService
{
    public ParsingService(ParsingProjectContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public void ParseChannelsData()
    {
        Console.WriteLine("Parsing data");
    }
}