
using DataProtection.Attributes;

namespace RegularApi.Domain.Model
{
    public class Host
    {
        public string Ip { get; set; }

        [Protected]
        public string Username { get; set; }

        [Protected]
        public string Password { get; set; }
    }
}