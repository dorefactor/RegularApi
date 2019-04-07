using System.Collections.Generic;
using RegularApi.Dao.Model;

namespace RegularApi.Dao
{
    public interface IApplicationDao
    {
        IList<Application> GetApplications();
        Application GetApplicationByName(string name);
    }
}