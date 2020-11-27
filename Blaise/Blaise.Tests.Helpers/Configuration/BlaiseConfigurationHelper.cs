using System;
using System.Configuration;
using System.IO;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Tests.Helpers.Configuration
{
    public class BlaiseConfigurationHelper
    {
        private static BlaiseConfigurationHelper _currentInstance;

        public static BlaiseConfigurationHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new BlaiseConfigurationHelper());
        }

        public string ServerParkName => GetVariable("ServerParkName");
        public string InstrumentPath => GetVariable("InstrumentPath");
        public string InstrumentName => GetVariable("InstrumentName");
        public string InstrumentExtension => GetVariable("InstrumentExtension");

        public ConnectionModel BuildConnectionModel()
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

        public string InstrumentPackage()
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
