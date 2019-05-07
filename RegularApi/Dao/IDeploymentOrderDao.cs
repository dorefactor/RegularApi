using System.Threading.Tasks;
using LanguageExt;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public interface IDeploymentOrderDao
    {
        Task<Option<DeploymentOrder>> SaveAsync(DeploymentOrder deploymentOrder);
        Task<Option<DeploymentOrder>> GetDeploymentOrderByRequestIdAsync(string id);
    }
}
