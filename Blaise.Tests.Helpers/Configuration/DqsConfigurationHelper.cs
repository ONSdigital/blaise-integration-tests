namespace Blaise.Tests.Helpers.Configuration
{
    using Blaise.Tests.Helpers.Framework.Extensions;

    public static class DqsConfigurationHelper
    {
        public static string DqsUrl => $"{ConfigurationExtensions.GetVariable("ENV_DQS_URL")}";

        public static string UploadUrl => $"{DqsUrl}/deploy";

        public static string UploadSummaryUrl => UploadUrl;

        public static string QuestionnaireExistsUrl => UploadUrl;

        public static string CannotOverwriteUrl => UploadUrl;

        public static string ConfirmOverwriteUrl => UploadUrl;

        public static string ConfirmDeleteUrl => $"{DqsUrl}/questionnaire";
    }
}
