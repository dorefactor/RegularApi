using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class ApplicationTransformer : BaseTransformer, ITransformer<ApplicationView, Application>
    {
        private readonly ITransformer<ApplicationSetupView, ApplicationSetup> _applicationSetupTransformer;

        public ApplicationTransformer(ITransformer<ApplicationSetupView, ApplicationSetup> applicationSetupTransformer)
        {
            _applicationSetupTransformer = applicationSetupTransformer;
        }

        public Application Transform(ApplicationView applicationView)
        {
            var application = new Application
            {
                Name = applicationView.Name,
                ApplicationSetup = _applicationSetupTransformer.Transform(applicationView.ApplicationSetupView)
            };

            // Id
            if (applicationView.Id != null)
            {
                application.Id = new ObjectId(applicationView.Id);
            }

            return application;
        }

        public ApplicationView Transform(Application application)
        {
            var applicationView = new ApplicationView
            {
                Name = application.Name,
                ApplicationSetupView = _applicationSetupTransformer.Transform(application.ApplicationSetup)
            };

            // Id
            if (ObjectIdIsNotEmpty(application.Id))
            {
                applicationView.Id = application.Id.ToString();
            }

            return applicationView;
        }
    }
}
