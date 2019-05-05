using System.Linq;
using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class DeploymentOrderTransformer : ITransformer<DeploymentOrderRequestView, DeploymentOrder>
    {
        private readonly ITransformer<ApplicationSetupView, ApplicationSetup> _applicationSetupTransformer;

        public DeploymentOrderTransformer(ITransformer<ApplicationSetupView, ApplicationSetup> applicationSetupTransformer)
        {
            _applicationSetupTransformer = applicationSetupTransformer;
        }

        public DeploymentOrder Transform(DeploymentOrderRequestView deploymentOrderView)
        {
            return new DeploymentOrder
            {
                DeploymentTemplateId = new ObjectId(deploymentOrderView.DeploymentTemplateId),
                ApplicationSetup = _applicationSetupTransformer.Transform(deploymentOrderView.ApplicationSetupView),
                HostsSetup = deploymentOrderView.HostSetupViews?.Select(hostSetupView => new HostSetup()
                {
                    Tag = hostSetupView.Tag,
                    Hosts = hostSetupView.Hosts?.Select(hostView => new Host()
                    {
                        Ip = hostView.Ip,
                        Username = hostView.Username,
                        Password = hostView.Password
                    }).ToList()
                }).ToList()
            };
        }

        public DeploymentOrderRequestView Transform(DeploymentOrder model)
        {
            throw new System.NotImplementedException();
        }
    }
}
