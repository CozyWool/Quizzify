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

        CreateMap<PackageEntity, PackageModel>()
            .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.PackageId))
            .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.PackageName))
            .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty))
            .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src => src.Rounds))
            .ReverseMap();

        CreateMap<RoundEntity, RoundModel>()
            .ForMember(dest => dest.RoundId, opt => opt.MapFrom(src => src.RoundId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Themes, opt => opt.MapFrom(src => src.Themes))
            .ReverseMap();
    }
}