using AutoWrapper.Wrappers;
using BuildingBlocks.Common.Models;
using BuildingBlocks.Common.Services;
using DesignSphere.Application.Contracts;
using DesignSphere.Application.Contracts.Services;
using DesignSphere.Domain.Entites;
using EventBus.Messages.Contract;
using static EventBus.Messages.Common.Enums.Enumeration;

namespace DesignSphere.Application.Features.Contributer;

public class AddProjectContributerRequest : IRequest<ApiResponse>
{
    public List<ProjectContributerRequest> ProjectContributers { get; set; }
}

public class AddProjectContributerCommandHandler : IRequestHandler<AddProjectContributerRequest, ApiResponse>

{
    private readonly IContributerRepository repository;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;
    private readonly IProjectRepository _projectRepository;

    public AddProjectContributerCommandHandler(IContributerRepository repository, IEmailService emailService,IUserService userService, IProjectRepository projectRepository)
    {
        this.repository = repository;
        _emailService = emailService;
        _userService = userService;
        _projectRepository = projectRepository;
    }


    public async Task<ApiResponse> Handle(AddProjectContributerRequest request, CancellationToken cancellationToken)
    {

        foreach (var projectContributer in request.ProjectContributers)
        {

            var contributer = new Domain.Entites.ProjectContributer
            {

                ContactId = projectContributer.ContactId,
                ProjectRoleId = projectContributer.ProjectRoleId,
                ProjectId = projectContributer.ProjectId
            };

            await repository.AddAsync(contributer);

            var project = await _projectRepository.GetByIdAsync(projectContributer.ProjectId);

            var messageBody = $"{_userService.GetFullName()} has added you to the design sphere Project {project.Name}";

            await _emailService.SendEmailAsync(projectContributer.Email, "Project Invitation", messageBody);
        }


        return new ApiResponse(200, "Contributer Successfully Added ");
    }
}
public class ProjectContributerRequest
{
    public Ulid ContactId { get; set; }
    public Ulid ProjectRoleId { get; set; }
    public Ulid ProjectId { get; set; }
    public string Email { get; set; }

}