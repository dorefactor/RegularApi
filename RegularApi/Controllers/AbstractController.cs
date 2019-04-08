using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RegularApi.Controllers.Views;

namespace RegularApi.Controllers
{
    public abstract class AbstractController : ControllerBase
    {
        protected ErrorResponse BuildErrorResponse(IList<string> errors)
        {
            return new ErrorResponse
            {
                Errors = errors
            };
        }
    }
}