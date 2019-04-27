using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public interface IApplicationDao
    {
        Task<IList<Application>> GetApplicationsAsync();
        Task<Option<Application>> GetApplicationByNameAsync(string name);
        Task<Option<Application>> SaveApplicationSetup(Application application);
    }
}