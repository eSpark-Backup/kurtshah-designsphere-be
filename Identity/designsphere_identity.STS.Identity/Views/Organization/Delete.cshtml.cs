using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
public class DeleteModel : PageModel
{
    private readonly IOrganizationRepository _organizationRepository;

    public DeleteModel(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    [BindProperty]
    public Organization Organization { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Organization = await _organizationRepository.GetOrganizationAsync(id);
        if (Organization == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        await _organizationRepository.DeleteOrganizationAsync(id);
        return RedirectToPage("./Index");
    }
}