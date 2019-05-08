using System.Threading.Tasks;
using LanguageExt;
using MongoDB.Bson;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public interface IDeploymentTemplateDao
    {
        Task<DeploymentTemplate> SaveAsync(DeploymentTemplate template);
        Task<Option<DeploymentTemplate>> GetByNameAsync(string templateName);
        Task<Option<DeploymentTemplate>> GetByIdAsync(ObjectId id);
    }
}