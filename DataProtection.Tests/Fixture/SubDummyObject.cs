using DataProtection.Attributes;

namespace DataProtection.Tests.Fixture
{
    public class SubDummyObject
    {
        public string Text { get; set; }
        
        [Protected]
        public string User { get; set; }
        
        [Protected]
        public string Password { get; set; }
    }
}