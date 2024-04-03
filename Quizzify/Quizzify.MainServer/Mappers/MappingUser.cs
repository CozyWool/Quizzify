using AutoMapper;
using Quizzify.DataAccess.Entities;
using Quizzify.MainServer.Models.Users;

namespace Quizzify.MainServer.Mappers;
public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<RegistrationModel, UserEntity>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.SelectedSecretQuestionId, opt => opt.MapFrom(src => src.SelectedSecretQuestionId))
            .ForMember(dest => dest.SecretAnswerHash, opt => opt.MapFrom(src => src.SecretAnswer));
    }
}