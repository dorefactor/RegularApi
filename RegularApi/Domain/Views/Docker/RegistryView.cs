namespace RegularApi.Domain.Views.Docker
{
    public class RegistryView
    {
        public bool IsPrivate { get; set; }

        public string Url { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
