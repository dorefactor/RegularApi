using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RegularApi.Tests
{
    public class BaseControllerIT : BaseIT
    {
        internal T GetPayloadViewFromJsonFile<T>(string jsonFilePath)
        {
            jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), jsonFilePath);

            var body = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<T>(body);
        }

        internal async Task<HttpResponseMessage> PerformPostAsync<T>(T view, string uri)
        {
            var body = JsonConvert.SerializeObject(view, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            return await HttpClient.PostAsync(uri, content);
        }

        internal async Task<HttpResponseMessage> PerformGetAsync(string uri)
        {
            return await HttpClient.GetAsync(uri);
        }

        internal async Task<T> GetResponseView<T>(HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
