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
                Id = deploymentTemplate.Id.ToString(),
                Name = deploymentTemplate.Name,
                ApplicationId = deploymentTemplate.ApplicationId.ToString(),
                ApplicationSetupView = _applicationSetupTransformer.Transform(deploymentTemplate.ApplicationSetup),
                HostSetupViews = ToView(deploymentTemplate.HostsSetup)
            };
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