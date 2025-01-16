
using AutoMapper;
using BuildingBlocks.Common.Models;
using DesignSphere.Application.Features.Contact;
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

            CreateMap<ProjectRole, GetProjectRoleQueryResult>().ReverseMap();

            CreateMap<ProjectRole, GetProjectRoleQueryResult>()
          .ForMember(dest => dest.ProjectRoleId, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Contact, GetContactQueryResult>().ReverseMap();

            //       CreateMap<Contact, GetContactQueryResult>()
            //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //.ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
            //.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<ProjectContributer, GetContributerQueryResult>().ReverseMap();

            CreateMap<ProjectContributer, GetContributerQueryResult>()
              .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.Contact.Name))
             .ForMember(dest => dest.ProjectRoleName, opt => opt.MapFrom(src => src.ProjectRole.Name));

            CreateMap<ProjectPlan, GetProjectPlansQueryResult>().ReverseMap();


        }
    }
}