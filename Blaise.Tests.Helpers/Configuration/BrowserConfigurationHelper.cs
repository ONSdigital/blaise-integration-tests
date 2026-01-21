namespace Blaise.Tests.Helpers.Configuration
{
    using Blaise.Tests.Helpers.Framework.Extensions;

    public static class BrowserConfigurationHelper
    {
        public static int TimeOutInSeconds => 90;

        public static string ChromeDriver => ConfigurationExtensions.GetEnvironmentVariable("ChromeWebDriver");
    }
}
