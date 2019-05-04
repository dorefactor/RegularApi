using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class DeploymentTemplateTransformer : ITransformer<DeploymentTemplateView, DeploymentTemplate>
    {
        public DeploymentTemplate Transform(DeploymentTemplateView deploymentTemplateView)
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

        public DeploymentTemplateView Transform(DeploymentTemplate deploymentTemplate)
        {
            return new DeploymentTemplateView
            {
                Name = deploymentTemplate.Name,
                ApplicationId = deploymentTemplate.ApplicationId.ToString(),
                EnvironmentVariables = deploymentTemplate.EnvironmentVariables,
                Ports = deploymentTemplate.Ports,
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
    }
}