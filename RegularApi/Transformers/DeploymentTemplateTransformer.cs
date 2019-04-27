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
                HostsSetup = FromResource(deploymentTemplateView.HostsSetup),
                Ports = deploymentTemplateView.Ports
            };
        }

        private HostSetup FromResource(HostSetupView hostsSetupView)
        {
            var hosts = from host in hostsSetupView.Hosts
                select new Host
                {
                    Ip = host.Ip,
                    Username = host.Username,
                    Password = host.Password
                };
            
            return new HostSetup
            {
                TagName = hostsSetupView.TagName,
                Hosts = hosts.ToList()
            };
        }
    }
}