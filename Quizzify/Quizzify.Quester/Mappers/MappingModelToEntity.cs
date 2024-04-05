using AutoMapper;
using Quizzify.DataAccess.Entities;
using Quizzify.Quester.Model.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzify.Quester.Mappers
{
    public class MappingModelToEntity : Profile
    {

        public MappingModelToEntity() 
        {
            CreateMap<PackageModel, PackageEntity>()
                .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.PackageId))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.PackageName))
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty))
                .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src => src.Rounds));

            CreateMap<RoundModel, RoundEntity>()
                .ForMember(dest => dest.RoundId, opt => opt.MapFrom(src => src.RoundId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoundName))
                .ForMember(dest => dest.RoundType, opt => opt.MapFrom(src => src.RoundType))
                .ForMember(dest => dest.Themes, opt => opt.MapFrom(src => src.Themes));

            CreateMap<ThemeModel, ThemeEntity>()
                .ForMember(dest => dest.ThemeId, opt => opt.MapFrom(src => src.ThemeId))
                .ForMember(dest => dest.ThemeName, opt => opt.MapFrom(src => src.ThemeName))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

            CreateMap<QuestionModel, QuestionEntity>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.QuestionCost, opt => opt.MapFrom(src => src.QuestionCost))
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.QuestionText))
                .ForMember(dest => dest.QuestionImageUrl, opt => opt.MapFrom(src => src.QuestionImageUrl))
                .ForMember(dest => dest.QuestionComment, opt => opt.MapFrom(src => src.QuestionComment))
                .ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText))
                .ForMember(dest => dest.AnswerImageUrl, opt => opt.MapFrom(src => src.AnswerImageUrl));



        }
    }
}
