using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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