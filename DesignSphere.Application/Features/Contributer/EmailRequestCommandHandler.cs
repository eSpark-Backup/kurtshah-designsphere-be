using AutoWrapper.Wrappers;
using DesignSphere.Application.Contracts;
using DesignSphere.Application.Contracts.Services;


namespace DesignSphere.Application.Features.Contributer;
public class SendEmailRequest : IRequest<ApiResponse>
{
    public List<EmailRequest> EmailRequests { get; set; }

}

public class EmailRequestCommandHandler : IRequestHandler<SendEmailRequest, ApiResponse>

{
    private readonly IEmailService _emailService;

    public EmailRequestCommandHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }


    public async Task<ApiResponse> Handle(SendEmailRequest request, CancellationToken cancellationToken)
    {
        foreach (var emailRequest in request.EmailRequests)
        {
            var url = $"http://localhost/Email?contactId={emailRequest.ContactId}&projectRoleId={emailRequest.ProjectRoleId}&projectId={emailRequest.ProjectId}";
            var messageBody = $"Please click the following link to view the details: <a href=\"{url}\">View Details</a>";

            await _emailService.SendEmailAsync(emailRequest.Email, "Project Invitation", messageBody);
        }

        return new ApiResponse(200, "Send Invite Delivered Successfully ");
    }
}
public class EmailRequest
{
    public Ulid ContactId { get; set; }
    public Ulid ProjectRoleId { get; set; }
    public Ulid ProjectId { get; set; }
    public string Email { get; set; }

}


