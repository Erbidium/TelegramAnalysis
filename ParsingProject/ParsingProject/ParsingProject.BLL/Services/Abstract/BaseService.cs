using AutoMapper;
using ParsingProject.DAL.Context;

namespace ParsingProject.BLL.Services.Abstract;

public abstract class BaseService
{
    protected readonly ParsingProjectContext _context;
    protected readonly IMapper _mapper;

    public BaseService(ParsingProjectContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
}