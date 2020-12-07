using System;
using System.Configuration;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class BlaiseConfigurationHelper
    {
        public static string ServerParkName => GetVariable("ServerParkName");
        public static string InstrumentPath => GetVariable("InstrumentPath");
        public static string InstrumentName => GetVariable("InstrumentName");
        public static string InstrumentPackage => $"{InstrumentPath}//{InstrumentName}.zip";

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

        private static string GetVariable(string variableName)
        {
            return ConfigurationManager.AppSettings[variableName];
        }

        private static int GetIntVariable(string variableName)
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings[variableName]);
        }
    }
}
