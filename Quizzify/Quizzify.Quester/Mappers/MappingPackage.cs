using AutoMapper;
using Quizzify.Quester.Model.Package;
using Quizzify.Quester.Model.Package.ViewModels;

namespace Quizzify.Quester.Mappers;

public class MappingPackage : Profile
{
    public MappingPackage()
    {
        CreateMap<PackageTreeViewModel, PackageModel>()
            .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Name));

        CreateMap<RoundTreeViewModel, RoundModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<QuestionTreeViewModel, QuestionModel>()
            .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.QuestionText));
    }
}