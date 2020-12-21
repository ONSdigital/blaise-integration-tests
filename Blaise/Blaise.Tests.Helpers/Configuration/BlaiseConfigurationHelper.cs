using System;
using System.Configuration;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Tests.Helpers.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class BlaiseConfigurationHelper
    {
        public static string ServerParkName => ConfigurationExtensions.GetVariable("ServerParkName");
        public static string InstrumentPath => ConfigurationExtensions.GetVariable("InstrumentPath");
        public static string InstrumentName => ConfigurationExtensions.GetVariable("InstrumentName");
        public static string SecondInstrumentName => ConfigurationExtensions.GetVariable("SecondInstrumentName");
        public static string InstrumentPackage => $"{InstrumentPath}//{InstrumentName}.zip";
        public static string SecondInstrumentPackage => $"{InstrumentPath}//{SecondInstrumentName}.zip";
        
        public static ConnectionModel BuildConnectionModel()
        {
            return new ConnectionModel{
                ServerName = ConfigurationExtensions.GetVariable("BlaiseServerHostName"),
                UserName = ConfigurationExtensions.GetVariable("BlaiseServerUserName"),
                Password = ConfigurationExtensions.GetVariable("BlaiseServerPassword"),
                Binding = ConfigurationExtensions.GetVariable("BlaiseServerBinding"),
                Port = ConfigurationExtensions.GetIntVariable("BlaiseConnectionPort"),
                RemotePort = ConfigurationExtensions.GetIntVariable("BlaiseRemoteConnectionPort"),
                ConnectionExpiresInMinutes = ConfigurationExtensions.GetIntVariable("ConnectionExpiresInMinutes")
            };
        }
    }
}
