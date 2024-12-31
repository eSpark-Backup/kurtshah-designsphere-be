
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
                .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => Enum.GetName(typeof(EProjectStage), src.Stage)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.GetName(typeof(EProjectType), src.Type)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.GetName(typeof(HelpOption), src.Type)));


        }
    }
}