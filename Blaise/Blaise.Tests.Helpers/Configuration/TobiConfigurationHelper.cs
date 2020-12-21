using Blaise.Tests.Helpers.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class TobiConfigurationHelper
    {
        public static string TobiUrl => ConfigurationExtensions.GetVariable("TobiUrl");
        public static string SurveyUrl = $"{TobiUrl}/survey/{BlaiseConfigurationHelper.InstrumentName.Substring(0, 3)}";
    }
}
