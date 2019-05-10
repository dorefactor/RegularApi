using System.Collections.Generic;
using System.Linq;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class DeploymentTemplateTransformer : BaseTransformer, ITransformer<DeploymentTemplateView, DeploymentTemplate>
    {
        private readonly ITransformer<ApplicationView, Application> _applicationTransformer;

        public DeploymentTemplateTransformer(ITransformer<ApplicationView, Application> applicationTransformer)
        {
            _applicationTransformer = applicationTransformer;
        }

        public DeploymentTemplate Transform(DeploymentTemplateView deploymentTemplateView)
        {
            return new DeploymentTemplate
            {
                Name = deploymentTemplateView.Name,
                Application = _applicationTransformer.Transform(deploymentTemplateView.ApplicationView),
                HostsSetup = FromView(deploymentTemplateView.HostSetupViews)
            };
        }

        public DeploymentTemplateView Transform(DeploymentTemplate deploymentTemplate)
        {
            var deploymentTemplateView = new DeploymentTemplateView
            {
                Name = deploymentTemplate.Name,
                ApplicationView = _applicationTransformer.Transform(deploymentTemplate.Application),
                HostSetupViews = ToView(deploymentTemplate.HostsSetup)
            };

            // Id
            if (ObjectIdIsNotEmpty(deploymentTemplate.Id))
            {
                deploymentTemplateView.Id = deploymentTemplate.Id.ToString();
            }

            return deploymentTemplateView;
        }

        private IList<HostSetup> FromView(IList<HostSetupView> hostsSetupView)
        {
            return (from hostSetupView in hostsSetupView
                    select new HostSetup
                    {
                        Tag = hostSetupView.Tag,
                        Hosts = (from hostView in hostSetupView.HostViews
                                 select new Host
                                 {
                                     Ip = hostView.Ip,
                                     Username = hostView.Username,
                                     Password = hostView.Password
                                 }).ToList()
                    }).ToList();
        }

        private IList<HostSetupView> ToView(IList<HostSetup> hostsSetup)
        {
            return (from hostSetup in hostsSetup
                    select new HostSetupView
                    {
                        Tag = hostSetup.Tag,
                        HostViews = (from host in hostSetup.Hosts
                                     select new HostView
                                     {
                                         Ip = host.Ip,
                                         Username = host.Username,
                                         Password = host.Password
                                     }).ToList()
                    }).ToList();
        }
    }
}