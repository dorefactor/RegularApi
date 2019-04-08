using System.Collections.Generic;

namespace RegularApi.Controllers.Deployment.Validators
{
    public interface IRequestValidator<T>
    {
        IList<string> Validate(T request);
    }
}