using AutoWrapper.Wrappers;
using DesignSphere.Application.Contracts;

namespace DesignSphere.Application.Features.Contact;

    public class DeleteContactRequest : IRequest<ApiResponse>
    {
        public Ulid Id { get; set; }

        public DeleteContactRequest(Ulid id) => Id = id;
    }

    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactRequest, ApiResponse>
    {
        private readonly IContactRepository _repository;

        public DeleteContactCommandHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse> Handle(DeleteContactRequest request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                return new ApiResponse(404, "Contact not found");
            }

            await _repository.DeleteAsync(entity);

            // Return a success response with a 204 No Content status
            return new ApiResponse(204, "Contact deleted successfully");
        }
    }
