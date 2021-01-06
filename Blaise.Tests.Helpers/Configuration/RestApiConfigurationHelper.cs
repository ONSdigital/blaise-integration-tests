using Blaise.Tests.Helpers.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class RestApiConfigurationHelper
    {
        public static string BaseUrl => ConfigurationExtensions.GetVariable("RestApiBaseUrl");
        public static string InstrumentsUrl =>
            $"/api/v1/serverparks/{BlaiseConfigurationHelper.ServerParkName}/instruments";
    }
}
