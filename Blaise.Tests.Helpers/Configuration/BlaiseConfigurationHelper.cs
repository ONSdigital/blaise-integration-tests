using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Tests.Helpers.Framework.Extensions;
using Blaise.Tests.Helpers.Questionnaire;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class BlaiseConfigurationHelper
    {
        public static string UninstallSurveyTimeOutInSeconds => ConfigurationExtensions.GetVariable("UninstallSurveyTimeOutInSeconds");
        public static string ServerParkName => ConfigurationExtensions.GetVariable("ServerParkName");
        public static string QuestionnairePath => ConfigurationExtensions.GetVariable("QuestionnairePath");
        public static string QuestionnaireName => ConfigurationExtensions.GetVariable("QuestionnaireName");
        public static string QuestionnairePackage => QuestionnaireHelper.QuestionnairePackagePath(QuestionnairePath, QuestionnaireName);

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
