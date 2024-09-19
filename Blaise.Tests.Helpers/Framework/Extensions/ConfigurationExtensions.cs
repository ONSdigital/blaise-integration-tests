using System;
using System.Configuration;

namespace Blaise.Tests.Helpers.Framework.Extensions
{
    public class ConfigurationExtensions
    {
        public static string GetEnvironmentVariable(string variableName)
        {
            var variable = Environment.GetEnvironmentVariable(variableName) ?? GetVariable(variableName);
            Console.WriteLine($"BENNY1 {variableName} = {variable}");
            variable.ThrowExceptionIfNullOrEmpty(variableName);
            return variable;
        }

        public static string GetVariable(string variableName)
        {
            var variable = ConfigurationManager.AppSettings[variableName];
            Console.WriteLine($"BENNY2 {variableName} = {variable}");
            variable.ThrowExceptionIfNullOrEmpty(variableName);
            return variable;    
        }

        public static int GetIntVariable(string variableName)
        {
            var variable = ConfigurationManager.AppSettings[variableName];
            Console.WriteLine($"BENNY3 {variableName} = {variable}");
            variable.ThrowExceptionIfNotInt(variableName);
            return Convert.ToInt32(variable);
        }
    }
}
