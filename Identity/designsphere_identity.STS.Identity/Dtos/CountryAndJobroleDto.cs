using System;
using System.Collections.Generic;

namespace designsphere_identity.STS.Identity.Dtos;

public class CountryAndJobroleDto
{
    public List<CountryDto> Countries { get; set; }
    public List<JobRoleDto> JobRoles { get; set; }

}


public class CountryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class JobRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}