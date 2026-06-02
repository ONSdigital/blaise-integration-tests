namespace Blaise.Tests.Helpers.Configuration
{
    using System;
    using Blaise.Tests.Helpers.Framework.Extensions;

    public static class CatiConfigurationHelper
    {
        private static readonly string _adminPassword;
        private static readonly string _interviewerPassword;

        static CatiConfigurationHelper()
        {
            _adminPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
            _interviewerPassword = Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        public static string CatiAdminUsername => "DSTAdminUser";

        public static string CatiAdminPassword => _adminPassword;

        public static string AdminRole => "DST";

        public static string CatiInterviewUsername => "DSTTestUser";

        public static string CatiInterviewPassword => _interviewerPassword;

        public static string InterviewRole => "DST";

        public static string CatiBaseUrl
        {
            get
            {
                string baseUrl = ConfigurationExtensions.GetVariable("ENV_BLAISE_CATI_URL");
                if (!baseUrl.StartsWith("http://") && !baseUrl.StartsWith("https://"))
                {
                    baseUrl = "https://" + baseUrl;
                }

                return baseUrl;
            }
        }

        public static string LoginUrl => NormaliseUrl($"{CatiBaseUrl}/Blaise/Account/Login");

        public static string NewDashboardLoginUrl => NormaliseUrl($"{CatiBaseUrl}/BlaiseDashboard/Account/Login");

        public static string DaybatchUrl => NormaliseUrl($"{CatiBaseUrl}/Blaise/Daybatch");

        public static string NewDashboardDaybatchUrl => NormaliseUrl($"{CatiBaseUrl}/BlaiseDashboard/Cati/Daybatch");

        public static string SchedulerUrl => NormaliseUrl($"{CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}");

        public static string SpecificationUrl => NormaliseUrl($"{CatiBaseUrl}/Blaise/Specification");

        public static string SurveyUrl => NormaliseUrl($"{CatiBaseUrl}/Blaise");

        public static string CaseUrl => NormaliseUrl($"{CatiBaseUrl}/Blaise/CaseInfo/StartSurvey?url={CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}/&rp.KeyValue=");

        public static string CaseInfoUrl => NormaliseUrl($"{CatiBaseUrl}/Blaise/CaseInfo");

        public static string NewDashboardCaseInfoUrl => NormaliseUrl($"{CatiBaseUrl}/BlaiseDashboard/Cati/CaseInfo");

        private static string NormaliseUrl(string url)
        {
            return System.Text.RegularExpressions.Regex.Replace(url, "(?<!:)//", "/");
        }
    }
}
