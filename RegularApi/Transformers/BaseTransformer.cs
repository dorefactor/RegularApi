using System.Linq;
using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class BaseTransformer
    {
        internal bool ObjectIdIsNotEmpty(ObjectId objectId)
        {
            return objectId != null && !objectId.ToString().Equals("000000000000000000000000");
        }
    }
}