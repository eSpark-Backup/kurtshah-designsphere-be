using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using AutoMapper;

public class EditModel : PageModel
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public EditModel(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        await _organizationRepository.PutOrganizationAsync(Organization);
        return RedirectToPage("./Index");
    }
}