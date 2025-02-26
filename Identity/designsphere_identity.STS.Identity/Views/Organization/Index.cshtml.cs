using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
public class IndexModel : PageModel
{
    private readonly IOrganizationRepository _organizationRepository;

    public IndexModel(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public IEnumerable<Organization> Organizations { get; set; }

    public async Task OnGetAsync()
    {
        Organizations = await _organizationRepository.GetOrganizationsAsync();
    }
}