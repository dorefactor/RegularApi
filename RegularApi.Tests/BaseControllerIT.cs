using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RegularApi.Tests
{
    public class BaseControllerIT : BaseIT
    {
        protected async Task<HttpResponseMessage> PerformPostAsync<T>(T requestBody, string uri)
        {
            var json = JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await HttpClient.PostAsync(uri, content);
        }

        protected async Task<HttpResponseMessage> PerformGetAsync(string uri)
        {
            return await HttpClient.GetAsync(uri);
        }

        protected async Task<T> GetResponse<T>(HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
