using System;
using System.Configuration;
using Blaise.Tests.Helpers.Framework.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class CatiConfigurationHelper
    {
        private static readonly Guid _adminPassword;
        private static readonly Guid _interviewerPassword;

        private static readonly string _blaiseVersion;

        static CatiConfigurationHelper()
        {
            _adminPassword = Guid.NewGuid();
            _interviewerPassword = Guid.NewGuid();
            _blaiseVersion = ConfigurationManager.AppSettings["ENV_BLAISE_VERSION"];
        }

        public static string CatiAdminUsername => "DSTAdminUser";

        public static string CatiAdminPassword => _adminPassword.ToString();

        public static string AdminRole => "DST";

        public static string CatiInterviewUsername => "DSTTestUser";

        public static string CatiInterviewPassword => _interviewerPassword.ToString();

        public static string InterviewRole => "DST";

        public static string CatiBaseUrl
        {
            get
            {
                var baseUrl = ConfigurationExtensions.GetVariable("ENV_BLAISE_CATI_URL");
                return baseUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? baseUrl.TrimEnd('/')
                    : "https://" + baseUrl.TrimEnd('/');
            }
        }

        /// <summary>
        /// Gets resolves the correct Blaise path based on ENV_BLAISE_VERSION
        /// v14 => blaise
        /// v16 => BlaiseDashboard.
        /// </summary>
        private static string BlaisePath
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "blaise";

                    case "v16":
                        return "BlaiseDashboard";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        public static string LoginUrl => $"{CatiBaseUrl}/{BlaisePath}/account/login";

        public static string DayBatchUrl => $"{CatiBaseUrl}/{BlaisePath}/daybatch";

        public static string SchedulerUrl => $"{CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}";

        public static string SpecificationUrl => $"{CatiBaseUrl}/{BlaisePath}/specification";

        public static string SurveyUrl => $"{CatiBaseUrl}/{BlaisePath}";

        public static string CaseUrl =>
            $"{CatiBaseUrl}/{BlaisePath}/CaseInfo/StartSurvey?url={CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}/&rp.KeyValue=";

        public static string CaseInfoUrl => $"{CatiBaseUrl}/{BlaisePath}/CaseInfo";
    }
}
