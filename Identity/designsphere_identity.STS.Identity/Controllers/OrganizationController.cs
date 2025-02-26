using designsphere_identity.Admin.EntityFramework.Shared.Entities.Application;
using designsphere_identity.STS.Identity.CustomGrants.Repository.Contracts;
using designsphere_identity.STS.Identity.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace designsphere_identity.STS.Identity.Controllers;

[Authorize]
[SecurityHeaders]
public class OrganizationController : Controller
{
    private readonly IOrganizationRepository _organizationRepository;

    public OrganizationController(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var organizations = await _organizationRepository.GetOrganizationsAsync();
        return View(organizations);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Organization request)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        Organization response = await _organizationRepository.PostOrganizationAsync(request);
        return View(response);
    }

    [HttpPut]
    public async Task<IActionResult> PutOrganizationAsync(Organization request)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        bool response = await _organizationRepository.PutOrganizationAsync(request);
        return View(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOrganizationAsync(Guid orgId)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        bool response = await _organizationRepository.DeleteOrganizationAsync(orgId);
        return View(response);
    }
}
