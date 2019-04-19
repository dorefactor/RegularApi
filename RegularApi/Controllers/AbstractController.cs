using Microsoft.AspNetCore.Mvc;
using RegularApi.Controllers.Views;

namespace RegularApi.Controllers
{
    public abstract class AbstractController : ControllerBase
    {
        protected ErrorResponse BuildErrorResponse(string error)
        {
            return new ErrorResponse
            {
                Error = error
            };
        }
    }
}