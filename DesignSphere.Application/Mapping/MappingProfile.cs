
using AutoMapper;
using DesignSphere.Domain.Entites;  // Domain models
using EventBus.Messages.Contract;   // DTOs

namespace DesignSphere.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, IProject>().ReverseMap();

        }
    }
}