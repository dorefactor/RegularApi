using System.Collections.Generic;
using DataProtection.Attributes;

namespace DataProtection.Tests.Fixture
{
    public class DummyObject
    {
        [Protected]
        public string Text { get; set; }
        
        [Protected]
        public IDictionary<string, string> KeyValues { get; set; }
        
        public IList<SubDummyObject> DummyObjects { get; set; }
    }
}