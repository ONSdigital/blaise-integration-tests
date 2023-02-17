using Blaise.Tests.Helpers.Framework.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class BrowserConfigurationHelper
    {
        public static int TimeOutInSeconds => 600;
        public static string ChromeDriver => ConfigurationExtensions.GetEnvironmentVariable("ChromeWebDriver");
    }
}