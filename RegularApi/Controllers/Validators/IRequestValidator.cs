using System.Collections.Generic;

namespace RegularApi.Controllers.Validators
{
    public interface IRequestValidator<T>
    {
        IList<string> Validate(T request);
    }
}