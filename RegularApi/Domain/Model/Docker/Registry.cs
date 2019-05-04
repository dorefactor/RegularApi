namespace RegularApi.Domain.Model.Docker
{
    public class Registry
    {
        public bool IsPrivate { get; set; }

        public string Url { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
