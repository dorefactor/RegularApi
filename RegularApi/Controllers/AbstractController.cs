using Microsoft.AspNetCore.Mvc;
using RegularApi.Domain.Views;

namespace RegularApi.Domain
{
    public abstract class AbstractController : ControllerBase
    {
        protected ErrorResponseView BuildErrorResponse(string error)
        {
            return new ErrorResponseView
            {
                Error = error
            };
        }
    }
}