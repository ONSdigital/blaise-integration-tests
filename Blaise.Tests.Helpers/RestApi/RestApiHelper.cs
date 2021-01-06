using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Models.Questionnaire;
using Newtonsoft.Json;

namespace Blaise.Tests.Helpers.RestApi
{
    public class RestApiHelper
    {
        private static HttpClient _httpClient;
        private static RestApiHelper _currentInstance;

        public RestApiHelper()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri(RestApiConfigurationHelper.BaseUrl)};
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static RestApiHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new RestApiHelper());
        }

        public async Task<List<Questionnaire>> GetAllActiveQuestionnaires()
        {
            var questionnaires = await GetListOfObjectsASync<Questionnaire>(RestApiConfigurationHelper.InstrumentsUrl);
            return questionnaires != null ? questionnaires.Where(q => q.Status == SurveyStatusType.Active).ToList() : new List<Questionnaire>();
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
