using System;
using System.Configuration;
using System.IO;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class BlaiseConfigurationHelper
    {
        public static string ServerParkName => GetVariable("ServerParkName");
        public static string InstrumentPath => GetVariable("InstrumentPath");
        public static string InstrumentName => GetVariable("InstrumentName");
        public static string InstrumentExtension => GetVariable("InstrumentExtension");

        public static ConnectionModel BuildConnectionModel()
        {
            return new ConnectionModel{
                ServerName = GetVariable("BlaiseServerHostName"),
                UserName = GetVariable("BlaiseServerUserName"),
                Password = GetVariable("BlaiseServerPassword"),
                Binding = GetVariable("BlaiseServerBinding"),
                Port = GetIntVariable("BlaiseConnectionPort"),
                RemotePort = GetIntVariable("BlaiseRemoteConnectionPort"),
                ConnectionExpiresInMinutes = GetIntVariable("ConnectionExpiresInMinutes")
            };
        }

        public static string InstrumentPackage()
        {
            return Path.Combine(InstrumentPath, InstrumentName + InstrumentExtension);
        }

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
