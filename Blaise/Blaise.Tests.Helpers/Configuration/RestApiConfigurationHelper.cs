using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class RestApiConfigurationHelper
    {
        public static string BaseUrl => GetVariable("RestApiBaseUrl");

        private static string GetVariable(string variableName)
        {
            return ConfigurationManager.AppSettings[variableName];
        }
    }
}
