using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class DeploymentTemplateTransformer : ITransformer<DeploymentTemplateView, DeploymentTemplate>
    {
        private readonly ITransformer<ApplicationSetupView, ApplicationSetup> _applicationSetupTransformer;

        public DeploymentTemplateTransformer(ITransformer<ApplicationSetupView, ApplicationSetup> applicationSetupTransformer)
        {
            _applicationSetupTransformer = applicationSetupTransformer;
        }

        public DeploymentTemplate Transform(DeploymentTemplateView deploymentTemplateView)
        {
            return new DeploymentTemplate
            {
                Name = deploymentTemplateView.Name,
                ApplicationId = ObjectId.Parse(deploymentTemplateView.ApplicationId),
                ApplicationSetup = _applicationSetupTransformer.Transform(deploymentTemplateView.ApplicationSetupView),
                HostsSetup = FromView(deploymentTemplateView.HostSetupViews)
            };
        }

        public DeploymentTemplateView Transform(DeploymentTemplate deploymentTemplate)
        {
            return new DeploymentTemplateView
            {
                Name = deploymentTemplate.Name,
                ApplicationId = deploymentTemplate.ApplicationId.ToString(),
                ApplicationSetupView = _applicationSetupTransformer.Transform(deploymentTemplate.ApplicationSetup)
            };
        }

        private IList<HostSetup> FromView(IList<HostSetupView> hostsSetupView)
        {
            var hostsSetup = from hostSetupView in hostsSetupView
                             select new HostSetup
                             {
                                 Tag = hostSetupView.Tag,
                                 Hosts = FromView(hostSetupView.HostViews)
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