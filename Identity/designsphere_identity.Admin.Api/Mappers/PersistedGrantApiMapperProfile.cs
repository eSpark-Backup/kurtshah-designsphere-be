using AutoMapper;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Dtos.Grant;
using designsphere_identity.Admin.Api.Dtos.PersistedGrants;

namespace designsphere_identity.Admin.Api.Mappers;

public class PersistedGrantApiMapperProfile : Profile
{
    public PersistedGrantApiMapperProfile()
    {
        CreateMap<PersistedGrantDto, PersistedGrantApiDto>(MemberList.Destination);
        CreateMap<PersistedGrantDto, PersistedGrantSubjectApiDto>(MemberList.Destination);
        CreateMap<PersistedGrantsDto, PersistedGrantsApiDto>(MemberList.Destination);
        CreateMap<PersistedGrantsDto, PersistedGrantSubjectsApiDto>(MemberList.Destination);
    }
}
