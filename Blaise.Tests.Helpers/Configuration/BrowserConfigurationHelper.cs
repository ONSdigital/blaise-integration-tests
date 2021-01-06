using Blaise.Tests.Helpers.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class BrowserConfigurationHelper
    {
        public static int TimeOutInSeconds => 60;
        public static string ChromeDriver => ConfigurationExtensions.GetEnvironmentVariable("ChromeWebDriver");
    }
}