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
                ApplicationId = ObjectId.Parse(deploymentTemplateView.ApplicationId),
                EnvironmentVariables = deploymentTemplateView.EnvironmentVariables,
                HostsSetup = FromResource(deploymentTemplateView.HostsSetup),
                Ports = deploymentTemplateView.Ports
            };
        }

        public DeploymentTemplateView ToResource(DeploymentTemplate deploymentTemplate)
        {
            return new DeploymentTemplateView
            {
                Name = deploymentTemplate.Name,
                ApplicationId = deploymentTemplate.ApplicationId.ToString(),
                EnvironmentVariables = deploymentTemplate.EnvironmentVariables,
                Ports = deploymentTemplate.Ports,
                HostsSetup = ToResource(deploymentTemplate.HostsSetup)
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

        private HostSetupView ToResource(HostSetup hostsSetup)
        {
            var hosts = from host in hostsSetup.Hosts
                select new HostView
                {
                    Ip = host.Ip,
                    Username = host.Username,
                    Password = host.Password
                };

            return new HostSetupView
            {
                TagName = hostsSetup.TagName,
                Hosts = hosts.ToList()
            };
        }
    }
}