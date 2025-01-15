using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Services;
using DesignSphere.Application.Contracts;
using DesignSphere.Domain.Entites;
using EventBus.Messages.Contract;
using SieveCustom.Models;

using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Application.Features.Project;

public class CreateProjectCommandRequest : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public DateTime? PublishDate { get; set; }
    public DateTime? DeviverableDate { get; set; }
    public EProjectStage Stage { get; set; }
    public EProjectType Type { get; set; }
    public HelpOption HelpUs { get; set; }
    public string? Comment { get; set; }


}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommandRequest, ApiResponse>

{
    private readonly IProjectRepository repository;
    private readonly IUserService _userService;
   private readonly IContributerRepository _contributerRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IProjectRoleRepository _projectRoleRepository;

    public CreateProjectCommandHandler(IProjectRepository repository, IUserService userService
       , IContributerRepository contributerRepository, IContactRepository contactRepository, IProjectRoleRepository projectRoleRepository
        )
    {
        this.repository = repository;
        _userService = userService;
        _contributerRepository = contributerRepository;
    }


    public async Task<ApiResponse> Handle(CreateProjectCommandRequest request, CancellationToken cancellationToken)
    {

        var project = new Domain.Entites.Project
        {
            Name = request.Name,
            PublishDate = request.PublishDate,
            DeviverableDate = request.DeviverableDate,
            Stage = request.Stage,
            Type = request.Type,
            HelpUs = request.HelpUs,
            Comment = request.Comment,
            OwnerId = _userService.GetUserIdentity(),
            OwnerName = _userService.GetFullName(),
            CompanyName = _userService.GetCompanyName()

        };

        await repository.AddAsync(project);

        if (project.Id != null)
        {

            var getContactId = await _contactRepository.getContactByUserId(_userService.GetUserIdentity());

            var contact = new Domain.Entites.Contact();
            if (getContactId == null)
            {

                contact.Name = _userService.GetFullName();
                contact.EmailAddress = _userService.GetUserEmail();
                contact.UserId = _userService.GetUserIdentity();

                await _contactRepository.AddAsync(contact);
            }
            var getProjectRole = await _projectRoleRepository.getProjectRole("Owner");


            var contributer = new Domain.Entites.ProjectContributer
            {
                ContactId = getContactId != null?getContactId.Id :contact.Id,
                ProjectRoleId = getProjectRole.Id,
                ProjectId = project.Id
            };
            await _contributerRepository.AddAsync(contributer);
        }

        return new ApiResponse(200, "Project Successfully Added ");
    }
}
