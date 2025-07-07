using Blaise.Tests.Helpers.Framework.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class TobiConfigurationHelper
    {
        public static string TobiUrl => $"{ConfigurationExtensions.GetVariable("ENV_TOBI_URL")}/";

        public static string SurveyUrl = $"{TobiUrl}survey/{BlaiseConfigurationHelper.QuestionnaireName.Substring(0, 3)}";
    }
}
