using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using RegularApi.Dao.Model;

namespace RegularApi.Dao
{
    public interface IApplicationDao
    {
        Task<IList<Application>> GetApplicationsAsync();
        Task<Option<Application>> GetApplicationByNameAsync(string name);
    }
}