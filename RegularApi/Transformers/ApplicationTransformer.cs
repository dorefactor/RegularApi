using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class ApplicationTransformer : ITransformer<ApplicationView, Application>
    {
        private readonly ITransformer<ApplicationSetupView, ApplicationSetup> _applicationSetupTransformer;

        public ApplicationTransformer(ITransformer<ApplicationSetupView, ApplicationSetup> applicationSetupTransformer)
        {
            _applicationSetupTransformer = applicationSetupTransformer;
        }

        public Application Transform(ApplicationView applicationView)
        {
            return new Application
            {
                Name = applicationView.Name,
                ApplicationSetup = _applicationSetupTransformer.Transform(applicationView.ApplicationSetupView)
            };
        }

        public ApplicationView Transform(Application application)
        {
            return new ApplicationView
            {
                Id = application.Id.ToString(),
                Name = application.Name,
                ApplicationSetupView = _applicationSetupTransformer.Transform(application.ApplicationSetup)
            };
        }
    }
}
