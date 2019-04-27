using System.Threading.Tasks;
using LanguageExt;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public interface IDeploymentTemplateDao
    {
        Task<DeploymentTemplate> NewAsync(DeploymentTemplate template);
        Task<Option<DeploymentTemplate>> GetByNameAsync(string templateName);
    }
}