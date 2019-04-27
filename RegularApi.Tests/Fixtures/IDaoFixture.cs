using System.Threading.Tasks;
using MongoDB.Bson;
using RegularApi.Domain.Model;

namespace RegularApi.Tests.Fixtures
{
    public interface IDaoFixture
    {
        Task<Application> CreateApplication(string name);
        Task<Application> GetApplicationById(ObjectId id);
    }
}