using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public interface IApplicationDao
    {
        Task<IList<Application>> GetAllAsync();
        Task<Option<Application>> GetByNameAsync(string name);
        Task<Option<Application>> SaveAsync(Application application);
    }
}