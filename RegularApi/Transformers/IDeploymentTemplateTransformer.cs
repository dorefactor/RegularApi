using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public interface IDeploymentTemplateTransformer
    {
        DeploymentTemplate FromResource(DeploymentTemplateView deploymentTemplateView);
        DeploymentTemplateView ToResource(DeploymentTemplate DeploymentTemplate);
    }
}