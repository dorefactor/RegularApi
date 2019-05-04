using System.Linq;
using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class DeploymentOrderTransformer : ITransformer<DeploymentOrderView, DeploymentOrder>
    {
        public DeploymentOrder Transform(DeploymentOrderView deploymentOrderView)
        {
            return new DeploymentOrder
            {
                DeploymentTemplateId = new ObjectId(deploymentOrderView.DeploymentTemplateId),
                ApplicationVersion = deploymentOrderView.Version,
                HostsSetup = deploymentOrderView.HostSetupViews?.Select(hostSetupView => new HostSetup()
                {
                    TagName = hostSetupView.TagName,
                    Hosts = hostSetupView.Hosts?.Select(hostView => new Host() // Change to HostViews
                    {
                        Ip = hostView.Ip,
                        Username = hostView.Username,
                        Password = hostView.Password
                    }).ToList()
                }).ToList()
            };
        }

        public DeploymentOrderView Transform(DeploymentOrder model)
        {
            throw new System.NotImplementedException();
        }
    }
}
