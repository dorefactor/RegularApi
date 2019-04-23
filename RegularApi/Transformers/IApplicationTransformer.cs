using RegularApi.Controllers.Dashboard.Models;
using RegularApi.Dao.Model;

namespace RegularApi.Transformers
{
    public interface IApplicationTransformer
    {

        Application fromResource(ApplicationResource applicationResource);
    }
}
