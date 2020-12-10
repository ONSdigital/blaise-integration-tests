using System.Configuration;
using Blaise.Tests.Helpers.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class RestApiConfigurationHelper
    {
        public static string BaseUrl => ConfigurationExtensions.GetVariable("RestApiBaseUrl");
    }
}
