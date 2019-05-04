using System.Threading.Tasks;
using LanguageExt;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public interface IDeploymentOrderDao
    {
        Task<Option<DeploymentOrder>> Save(DeploymentOrder deploymentOrder);
        Task<Option<DeploymentOrderDetailVo>> GetDeploymentOrderDetailByRequestIdAsync(string id);
    }
}
