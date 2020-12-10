using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Models;
using Newtonsoft.Json;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Tests.Helpers.RestApi
{
    public class RestApiHelper
    {
        private static HttpClient _httpClient;

        public RestApiHelper()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri(RestApiConfigurationHelper.BaseUrl)};
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<List<Questionnaire>> GetAllActiveQuestionnaires()
        {
            return await GetListOfObjectsASync<Questionnaire>("urlForRestApi");
        }

        private static async Task<List<T>> GetListOfObjectsASync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode != HttpStatusCode.OK) return default;

            var responseAsJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<T>>(responseAsJson);
        }
    }
}
