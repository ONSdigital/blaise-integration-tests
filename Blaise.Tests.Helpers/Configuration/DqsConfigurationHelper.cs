using Blaise.Tests.Helpers.Framework.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class DqsConfigurationHelper
    {
        public static string DqsUrl => ConfigurationExtensions.GetVariable("ENV_DQS_URL");
        public static string UploadUrl => $"{DqsUrl}/upload";
        public static string UploadSummaryUrl => $"{DqsUrl}/UploadSummary";
        public static string QuestionnaireExistsUrl => $"{UploadUrl}/survey-exists";
    }
}
