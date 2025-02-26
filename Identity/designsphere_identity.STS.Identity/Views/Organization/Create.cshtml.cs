using AutoMapper;
using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
public class CreateModel : PageModel
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public CreateModel(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    [BindProperty]
    public Organization Organization { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        await _organizationRepository.PostOrganizationAsync(Organization);
        return RedirectToPage("./Index");
    }
}