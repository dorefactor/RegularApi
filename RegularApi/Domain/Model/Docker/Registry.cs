using DoRefactor.AspNetCore.DataProtection.Attributes;

namespace RegularApi.Domain.Model.Docker
{
    public class Registry
    {
        public bool IsPrivate { get; set; }

        public string Url { get; set; }

        [Protected]
        public string Username { get; set; }

        [Protected]
        public string Password { get; set; }
    }
}
