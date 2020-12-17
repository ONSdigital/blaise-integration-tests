using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Tests.Helpers.Extensions
{
    public class ConfigurationExtensions
    {
        public static string GetEnvironmentVariable(string variableName)
        {
            var variable = Environment.GetEnvironmentVariable(variableName) ?? GetVariable(variableName);
            variable.ThrowExceptionIfNullOrEmpty(variableName);
            return variable;
        }

        public static string GetVariable(string variableName)
        {
            var variable = ConfigurationManager.AppSettings[variableName];
            variable.ThrowExceptionIfNullOrEmpty(variableName);
            return variable;
        }

        public static int GetIntVariable(string variableName)
        {
            var variable = ConfigurationManager.AppSettings[variableName];
            variable.ThrowExceptionIfNotInt(variableName);
            return Convert.ToInt32(variable);
        }
    }
}
