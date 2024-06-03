using AutoMapper;
using ParsingProject.BLL.Entities;
using ParsingProject.DAL.Entities;

namespace ParsingProject.BLL.MappingProfiles;

public class ChannelProfile : Profile
{
    public ChannelProfile()
    {
        CreateMap<TL.Channel, Channel>()
            .ForMember(dest => dest.MainUsername, opt => opt.MapFrom(src => src.MainUsername))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.TelegramId, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.ParticipantsCount, opt => opt.MapFrom(src => src.participants_count));

        CreateMap<Channel, ChannelModel>();
    }
}