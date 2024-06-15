using AutoMapper;
using ParsingProject.DAL.Entities;
using TL;

namespace ParsingProject.BLL.MappingProfiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Message, Comment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TelegramId, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.message))
            .ForMember(dest => dest.Hash, opt => opt.MapFrom(src => src.message.GetHashCode()))
            .ForMember(dest => dest.ViewsCount, opt => opt.MapFrom(src => src.views))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.EditedAt, opt => opt.MapFrom(src => src.edit_date))
            .ForMember(dest => dest.PostId, opt => opt.Ignore())
            .ForMember(dest => dest.ParsedAt, opt => opt.MapFrom(_ => DateTime.Now));
    }
}