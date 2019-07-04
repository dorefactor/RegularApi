using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace MongoDataProtection.Repository
{
    public sealed class MongoXmlRepository : IXmlRepository
    {
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            throw new System.NotImplementedException();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            throw new System.NotImplementedException();
        }
    }


}