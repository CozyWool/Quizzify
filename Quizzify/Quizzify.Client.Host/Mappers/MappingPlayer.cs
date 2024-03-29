using AutoMapper;
using Quizzify.Client.Host.Model;
using Quizzify.DataAccess.Entities;

namespace Quizzify.Client.Host.Mappers;
public class MappingPlayer : Profile
{
    public MappingPlayer()
    {
        CreateMap<PlayerEntity, PlayerModel>()
            .ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.PlayerId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Nickname))
            .ForMember(dest => dest.UserProfilePicture, opt => opt.MapFrom(src => src.UserProfilePicture))
            .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About))
            .ReverseMap();
    }
}