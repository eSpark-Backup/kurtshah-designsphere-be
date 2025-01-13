
using AutoMapper;
using DesignSphere.Application.Features.Contributer;
using DesignSphere.Application.Features.Plan;
using DesignSphere.Application.Features.Project;
using DesignSphere.Domain.Entites;
using EventBus.Messages.Contract;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, GetProjectQueryResult>().ReverseMap();

            CreateMap<Project, GetProjectQueryResult>()
               // .ForMember(dest => dest.ProjectPlans, opt => opt.MapFrom(src => src.ProjectPlans))
                .ForMember(dest => dest.DoorOpenings, opt => opt.MapFrom(src => src.ProjectPlans.Sum(pp => pp.DoorOpenings.Count)))
                .ForMember(dest => dest.StageName, opt => opt.MapFrom(src => Enum.GetName(typeof(EProjectStage), src.Stage)))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => Enum.GetName(typeof(EProjectType), src.Type)))
                .ForMember(dest => dest.HelpUsName, opt => opt.MapFrom(src => Enum.GetName(typeof(HelpOption), src.Type)));

            CreateMap<ProjectContributer, GetContributerQueryResult>().ReverseMap();

            CreateMap<ProjectContributer, GetContributerQueryResult>()
                .ForMember(dest => dest.ProjectUserRoleName, opt => opt.MapFrom(src => Enum.GetName(typeof(ContributerRole), src.ProjectUserRole)))
                .ForMember(dest => dest.ProjectRoleName, opt => opt.MapFrom(src => Enum.GetName(typeof(EProjectRole), src.ProjectRole)));
            
            CreateMap<ProjectPlan, GetProjectPlansQueryResult>().ReverseMap();


        }
    }
}