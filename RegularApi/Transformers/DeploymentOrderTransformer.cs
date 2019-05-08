using System.Linq;
using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class DeploymentOrderTransformer : ITransformer<DeploymentOrderView, DeploymentOrder>
    {
        private readonly ITransformer<ApplicationSetupView, ApplicationSetup> _applicationSetupTransformer;

        public DeploymentOrderTransformer(ITransformer<ApplicationSetupView, ApplicationSetup> applicationSetupTransformer)
        {
            _applicationSetupTransformer = applicationSetupTransformer;
        }

        public DeploymentOrder Transform(DeploymentOrderView deploymentOrderView)
        {
            return new DeploymentOrder
            {
                DeploymentTemplateId = new ObjectId(deploymentOrderView.DeploymentTemplateId),
                ApplicationSetup = _applicationSetupTransformer.Transform(deploymentOrderView.ApplicationSetupView),
                HostsSetup = deploymentOrderView.HostSetupViews?.Select(hostSetupView => new HostSetup()
                {
                    Tag = hostSetupView.Tag,
                    Hosts = hostSetupView.HostViews?.Select(hostView => new Host()
                    {
                        Ip = hostView.Ip,
                        Username = hostView.Username,
                        Password = hostView.Password
                    }).ToList()
                }).ToList()
            };
        }

        public DeploymentOrderView Transform(DeploymentOrder deploymentOrder)
        {
            var deploymentOrderView = new DeploymentOrderView
            {
                DeploymentTemplateId = deploymentOrder.DeploymentTemplateId.ToString(),
                RequestId = deploymentOrder.RequestId,
                CreatedAt = deploymentOrder.CreatedAt.ToString(),
                HostSetupViews = deploymentOrder.HostsSetup?.Select(hostSetup => new HostSetupView()
                {
                    Tag = hostSetup.Tag,
                    HostViews = hostSetup.Hosts?.Select(host => new HostView()
                    {
                        Ip = host.Ip,
                        Username = host.Username,
                        Password = host.Password
                    }).ToList()
                }).ToList()
            };

            // ApplicationSetupView
            if (deploymentOrder.ApplicationSetup != null)
            {
                deploymentOrderView.ApplicationSetupView = _applicationSetupTransformer.Transform(deploymentOrder.ApplicationSetup);
            }

            return deploymentOrderView;
        }
    }
}