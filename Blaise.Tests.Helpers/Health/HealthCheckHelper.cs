using Blaise.Nuget.Api.Api;
using System;
using System.Net.Http;

namespace Blaise.Tests.Helpers.Health
{
    public static class HealthCheckHelper
    {
        public static void CheckUrlIsAvailable(string url)
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
                throw new Exception($"Failed to connect to the URL '{url}'. Please check the application is running and the URL is correct. Inner exception: {ex.Message}", ex);
            }
        }

        public static void CheckBlaiseConnection()
        {
            try
            {
                var blaiseApi = new BlaiseQuestionnaireApi();
                blaiseApi.GetServerParks();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to connect to the Blaise server. Please check the connection details are correct and the server is running. Inner exception: {ex.Message}", ex);
            }
        }
    }
}
