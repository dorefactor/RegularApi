using System.Collections.Generic;
using MongoDB.Driver;
using RegularApi.Dao.Model;

namespace RegularApi.Dao
{
    public class ApplicationDao : IApplicationDao
    {
        private readonly IMongoClient _mongoClient;

        public ApplicationDao(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }
        
        public IList<Application> GetApplications()
        {
            var database = _mongoClient.GetDatabase("regular-deployer");

            var collection = database.GetCollection<Application>("applications");

            return collection.Find(FilterDefinition<Application>.Empty).ToList();
        }

        public Application GetApplicationByName(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}