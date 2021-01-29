using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Tests.Helpers.Framework.Extensions;

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
                ServerName = ConfigurationExtensions.GetVariable("ENV_BLAISE_SERVER_HOST_NAME"),
                UserName = ConfigurationExtensions.GetVariable("ENV_BLAISE_ADMIN_USER"),
                Password = ConfigurationExtensions.GetVariable("ENV_BLAISE_ADMIN_PASSWORD"),
                Binding = ConfigurationExtensions.GetVariable("ENV_BLAISE_SERVER_BINDING"),
                Port = ConfigurationExtensions.GetIntVariable("ENV_BLAISE_CONNECTION_PORT"),
                RemotePort = ConfigurationExtensions.GetIntVariable("ENV_BLAISE_REMOTE_CONNECTION_PORT"),
                ConnectionExpiresInMinutes = ConfigurationExtensions.GetIntVariable("ENV_CONNECTION_EXPIRES_IN_MINUTES")
            };
        }
    }
}
