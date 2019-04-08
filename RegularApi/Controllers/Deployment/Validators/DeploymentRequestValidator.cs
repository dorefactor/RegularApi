using System;
using System.Collections.Generic;
using RegularApi.Controllers.Deployment.Views;
using RegularApi.Controllers.Validators;

namespace RegularApi.Controllers.Deployment.Validators
{
    public class DeploymentRequestValidator : IRequestValidator<ApplicationRequest>
    {
        public IList<string> Validate(ApplicationRequest request)
        {
            var errors = new List<string>();

            if (request == null)
            {
                errors.Add("Application cannot be null");
                return errors;
            }
            
            if (String.IsNullOrEmpty(request.Name))
                errors.Add("Name is required");
            
            if (String.IsNullOrEmpty(request.Tag))
                errors.Add("Tag is required");

            return errors;
        }
    }
}