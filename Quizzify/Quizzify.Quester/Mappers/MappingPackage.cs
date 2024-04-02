using AutoMapper;
using Quizzify.Quester.Model.Package;
using Quizzify.Quester.Model.Package.ViewModels;

namespace Quizzify.Quester.Mappers;

public class MappingPackage : Profile
{
    public MappingPackage()
    {
        CreateMap<PackageTreeViewModel, PackageModel>()
            .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Rounds, opt => opt.MapFrom(src => src.Rounds));

        CreateMap<RoundTreeViewModel, RoundModel>();
        CreateMap<QuestionTreeViewModel, QuestionModel>();
    }
}