using System.Linq;
using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class DeploymentOrderTransformer : ITransformer<DeploymentOrderView, DeploymentOrder>
    {
        private readonly ITransformer<ApplicationView, Application> _applicationTransformer;

        public DeploymentOrderTransformer(ITransformer<ApplicationView, Application> applicationTransformer)
        {
            _applicationTransformer = applicationTransformer;
        }

        public DeploymentOrder Transform(DeploymentOrderView deploymentOrderView)
        {
            return new DeploymentOrder
            {
                DeploymentTemplateId = new ObjectId(deploymentOrderView.DeploymentTemplateId),
                Application = _applicationTransformer.Transform(deploymentOrderView.ApplicationView),
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
                RequestId = deploymentOrder.RequestId,
                CreatedAt = deploymentOrder.CreatedAt,
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
            if (deploymentOrder.Application?.ApplicationSetup != null)
            {
                deploymentOrderView.ApplicationView = _applicationTransformer.Transform(deploymentOrder.Application);
            }

            return deploymentOrderView;
        }
    }
}