using designsphere_identity.Admin.Api.ExceptionHandling;

namespace designsphere_identity.Admin.Api.Resources;

public class ApiErrorResources : IApiErrorResources
{
    public virtual ApiError CannotSetId()
    {
        return new ApiError
        {
            Code = nameof(CannotSetId),
            Description = ApiErrorResource.CannotSetId
        };
    }
}
