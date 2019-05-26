using System;
using FluentAssertions;
using NUnit.Framework;
using RegularApi.Factories;

namespace RegularApi.Tests.Factories
{
    public class HttpClientFactoryTest
    {
        [Datapoints]
        public string[] STRINGS = { "asdf", null, "" };

        private IHttpClientFactory _httpClientFactory;

        [Theory]
        public void TestCreateWithBasicAuth(string prefix)
        {
            var baseUrl = "http://baseUrl-" + prefix;
            var username = "username-" + prefix;
            var password = "password-" + prefix;

            var expectedAuthToken = BuildBasicAuthToken(username, password);

            _httpClientFactory = new HttpClientFactory(baseUrl, username, password);

            var actualHttpClient = _httpClientFactory.CreateWithBasicAuth();

            actualHttpClient.BaseAddress.Should().Equals(baseUrl);
            actualHttpClient.DefaultRequestHeaders.Authorization.Should().Equals(expectedAuthToken);
        }

        private string BuildBasicAuthToken(string username, string password)
        {
            var authToken = username + ":" + password;
            var authTokenInBytes = System.Text.Encoding.UTF8.GetBytes(authToken);

            return Convert.ToBase64String(authTokenInBytes);
        }
    }
}