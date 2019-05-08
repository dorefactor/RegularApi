using System.Threading.Tasks;
using LanguageExt;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public interface IDeploymentOrderDao
    {
        Task<DeploymentOrder> SaveAsync(DeploymentOrder deploymentOrder);
        Task<Option<DeploymentOrder>> GetByRequestIdAsync(string id);
    }
}
