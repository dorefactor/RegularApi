using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public interface IDeploymentTemplateTransformer
    {
        DeploymentTemplate FromView(DeploymentTemplateView deploymentTemplateView);
        DeploymentTemplateView ToView(DeploymentTemplate DeploymentTemplate);
    }
}