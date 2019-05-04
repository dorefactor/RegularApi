using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class DeploymentTemplateTransformer : IDeploymentTemplateTransformer
    {
        public DeploymentTemplate FromView(DeploymentTemplateView deploymentTemplateView)
        {
            return new DeploymentTemplate
            {
                Name = deploymentTemplateView.Name,
                ApplicationId = ObjectId.Parse(deploymentTemplateView.ApplicationId),
                EnvironmentVariables = deploymentTemplateView.EnvironmentVariables,
                HostsSetup = FromView(deploymentTemplateView.HostSetupViews),
                Ports = deploymentTemplateView.Ports
            };
        }

        public DeploymentTemplateView ToView(DeploymentTemplate deploymentTemplate)
        {
            return new DeploymentTemplateView
            {
                Name = deploymentTemplate.Name,
                ApplicationId = deploymentTemplate.ApplicationId.ToString(),
                EnvironmentVariables = deploymentTemplate.EnvironmentVariables,
                Ports = deploymentTemplate.Ports,
                // HostsSetup = ToResource(deploymentTemplate.HostsSetup)
            };
        }

        private IList<HostSetup> FromView(IList<HostSetupView> hostsSetupView)
        {
            var hostsSetup = from hostSetupView in hostsSetupView
                select new HostSetup
                {
                    TagName = hostSetupView.TagName,
                    Hosts = FromView(hostSetupView.Hosts)
                };

            return hostsSetup.ToList();
        }

        private IList<Host> FromView(IList<HostView> hostsView)
        {
            var hosts = from hostView in hostsView
                select new Host
                {
                    Ip = hostView.Ip,
                    Username = hostView.Username,
                    Password = hostView.Password
                };

            return hosts.ToList();
        }

        private IList<HostSetupView> ToView(IList<HostSetup> hostsSetup)
        {
            var hostsSetupView = from hostSetup in hostsSetup
                select new HostSetupView
                {
                    TagName = hostSetup.TagName,
                    Hosts = ToView(hostSetup.Hosts)
                };

            return hostsSetupView.ToList();
        }

        private IList<HostView> ToView(IList<Host> hosts)
        {
            var hostsView = from host in hosts
                select new HostView
                {
                    Ip = host.Ip,
                    Username = host.Username,
                    Password = host.Password
                };

            return hostsView.ToList();
        }
    }
}