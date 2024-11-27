
using AutoMapper;
using DesignSphere.Application.Features.Project;
using DesignSphere.Domain.Entites; 

namespace DesignSphere.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, GetProjectQueryResult>().ReverseMap();

        }
    }
}