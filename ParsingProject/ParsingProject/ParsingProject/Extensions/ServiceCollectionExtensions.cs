using System.Reflection;
using FluentValidation;
using ParsingProject.BLL.MappingProfiles;
using ParsingProject.Validators;

namespace ParsingProject.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(ChannelProfile)));
    }
    
    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ChannelsParsingDtoValidator>();
    }
}