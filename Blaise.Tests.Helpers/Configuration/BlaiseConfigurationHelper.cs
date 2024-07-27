using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Tests.Helpers.Framework.Extensions;
using Blaise.Tests.Helpers.Questionnaire;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class BlaiseConfigurationHelper
    {
        public static string UninstallSurveyTimeOutInSeconds => ConfigurationExtensions.GetVariable("UninstallSurveyTimeOutInSeconds");
        public static string ServerParkName => ConfigurationExtensions.GetVariable("ServerParkName");
        public static string InstrumentPath => ConfigurationExtensions.GetVariable("InstrumentPath");
        public static string InstrumentName => ConfigurationExtensions.GetVariable("InstrumentName");
        public static string InstrumentPackage => QuestionnaireHelper.InstrumentPackagePath(InstrumentPath, InstrumentName);

        public static ConnectionModel BuildConnectionModel()
        {
            return new ConnectionModel{
                ServerName = ConfigurationExtensions.GetVariable("ENV_BLAISE_SERVER_HOST_NAME"),
                UserName = ConfigurationExtensions.GetVariable("ENV_BLAISE_ADMIN_USER"),
                Password = ConfigurationExtensions.GetVariable("ENV_BLAISE_ADMIN_PASSWORD"),
                Binding = ConfigurationExtensions.GetVariable("ENV_BLAISE_SERVER_BINDING"),
                Port = ConfigurationExtensions.GetIntVariable("ENV_BLAISE_CONNECTION_PORT"),
                RemotePort = ConfigurationExtensions.GetIntVariable("ENV_BLAISE_REMOTE_CONNECTION_PORT"),
                ConnectionExpiresInMinutes = ConfigurationExtensions.GetIntVariable("ENV_CONNECTION_EXPIRES_IN_MINUTES"),
            };
        }
    }
}
