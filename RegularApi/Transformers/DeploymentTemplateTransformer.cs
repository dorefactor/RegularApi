using System.Linq;
using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class DeploymentTemplateTransformer : IDeploymentTemplateTransformer
    {
        public DeploymentTemplate FromResource(DeploymentTemplateView deploymentTemplateView)
        {
            return new DeploymentTemplate
            {
                Name = deploymentTemplateView.Name,
                ApplicationId = new ObjectId(deploymentTemplateView.ApplicationId),
                EnvironmentVariables = deploymentTemplateView.EnvironmentVariables,
                HostsSetup = deploymentTemplateView.HostsSetupViews?.Select(hostSetupView => new HostSetup // Change to HostViews
                {
                    TagName = hostSetupView.TagName,
                    Hosts = hostSetupView.Hosts?.Select(hostView => new Host
                    {
                        Ip = hostView.Ip,
                        Username = hostView.Username,
                        Password = hostView.Password
                    }).ToList()
                }).ToList(),
                Ports = deploymentTemplateView.Ports
            };
        }
    }
}