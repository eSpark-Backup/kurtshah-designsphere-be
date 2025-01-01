
using AutoMapper;
using DesignSphere.Application.Features.Project;
using DesignSphere.Domain.Entites;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, GetProjectQueryResult>().ReverseMap();

            CreateMap<Project, GetProjectQueryResult>()
                .ForMember(dest => dest.StageName, opt => opt.MapFrom(src => Enum.GetName(typeof(EProjectStage), src.Stage)))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => Enum.GetName(typeof(EProjectType), src.Type)))
                .ForMember(dest => dest.HelpUsName, opt => opt.MapFrom(src => Enum.GetName(typeof(HelpOption), src.Type)));


        }
    }
}