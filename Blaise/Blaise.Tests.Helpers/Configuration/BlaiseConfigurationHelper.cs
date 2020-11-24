using System;
using System.Configuration;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Tests.Helpers.Configuration
{
    public class BlaiseConfigurationHelper
    {
        public string ServerParkName => GetVariable("ServerParkName");
        public string InstrumentPath => GetVariable("InstrumentPath");
      
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
