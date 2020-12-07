using System;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class BrowserConfigurationHelper
    {
        public static int TimeOutInSeconds => 60;
        public static string ChromeDriver => Environment.GetEnvironmentVariable("ChromeWebDriver");
    }
}