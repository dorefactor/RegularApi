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
            var token = BuildBasicAuthToken(_username, _password);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

            return httpClient;
        }

        private string BuildBasicAuthToken(string username, string password)
        {
            var token = username + ":" + password;
            var tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(tokenBytes);
        }
    }
}
