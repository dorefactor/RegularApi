using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RegularApi.Factories
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly string _baseUrl;
        private readonly string _username;
        private readonly string _password;

        public HttpClientFactory(string baseUrl, string username, string password)
        {
            _baseUrl = baseUrl;
            _username = username;
            _password = password;
        }

        public HttpClient CreateWithBasicAuth()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            var authToken = BuildBasicAuthToken(_username, _password);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            return httpClient;
        }

        private string BuildBasicAuthToken(string username, string password)
        {
            var authToken = username + ":" + password;
            var authTokenInBytes = System.Text.Encoding.UTF8.GetBytes(authToken);

            return Convert.ToBase64String(authTokenInBytes);
        }
    }
}
