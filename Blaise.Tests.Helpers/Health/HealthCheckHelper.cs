namespace Blaise.Tests.Helpers.Health
{
    using System;
    using System.Net.Http;
    using Blaise.Nuget.Api.Api;
    using Blaise.Tests.Helpers.Configuration;

    public static class HealthCheckHelper
    {
        public static void CheckUrl(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    var response = client.GetAsync(url).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"The URL '{url}' is not available. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to connect to the URL '{url}'. Check the application is running and the URL is correct. Error: {ex.Message}");
            }
        }

        public static void CheckBlaiseConnection()
        {
            try
            {
                var blaiseApi = new BlaiseQuestionnaireApi();
                blaiseApi.QuestionnaireExists(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to connect to Blaise. Check Blaise and rproxy are running and connection details are corrct. Error: {ex.Message}");
            }
        }
    }
}
