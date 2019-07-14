using System.Collections.Generic;

namespace DataProtection.Tests.Fixture
{
    public class DummyObjectFixture
    {
        public static DummyObject BuildDummyObject(string text)
        {
            var subDummy = new SubDummyObject
            {
                Text = text,
                User = "user-name",
                Password = "something-secret"
            };

            var subDummy2 = new SubDummyObject
            {
                Text = text,
                User = "other-user",
                Password = "something not secret"
            };

            return new DummyObject
            {
                Text = text,
                KeyValues = new Dictionary<string, string>
                {
                    {"key-one", "value one"},
                    {"key-two", "value two"}
                },
                DummyObjects = new List<SubDummyObject>
                {
                    subDummy,
                    subDummy2
                }
            };
        }
    }
}