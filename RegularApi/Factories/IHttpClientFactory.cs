using System.Net.Http;

namespace RegularApi.Factories
{
    public interface IHttpClientFactory
    {
        HttpClient CreateWithBasicAuth();
    }
}
