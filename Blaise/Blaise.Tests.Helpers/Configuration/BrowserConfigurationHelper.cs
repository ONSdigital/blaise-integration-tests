using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class BrowserConfigurationHelper
    {
        public static int TimeOutInSeconds => GetIntVariable("TimeOutInSeconds");
        public static string ChromeDriver => GetVariable("ChromeDriver");

        private static string GetVariable(string variableName)
        {
            var value = ConfigurationManager.AppSettings[variableName];
            return value;
        }

        private static int GetIntVariable(string variableName)
        {
            var value = ConfigurationManager.AppSettings[variableName];
            return Convert.ToInt32(value);
        }
    }
}
