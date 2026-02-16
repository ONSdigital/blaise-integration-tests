namespace Blaise.Tests.Helpers.Configuration
{
    using System;
    using Blaise.Tests.Helpers.Framework.Extensions;

    public static class CatiConfigurationHelper
    {
        private static readonly Guid _adminPassword;
        private static readonly Guid _interviewerPassword;

        static CatiConfigurationHelper()
        {
            _adminPassword = Guid.NewGuid();
            _interviewerPassword = Guid.NewGuid();
        }

        public static string CatiAdminUsername => "DSTAdminUser";
        public static string CatiAdminPassword => $"{_adminPassword}";
        public static string AdminRole => "DST";
        public static string CatiInterviewUsername => "DSTTestUser";
        public static string CatiInterviewPassword => $"{_interviewerPassword}";
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

        public static string LoginUrl => $"{CatiBaseUrl}/Blaise/Account/Login";
        public static string NewDashboardLoginUrl => $"{CatiBaseUrl}/BlaiseDashboard/Account/Login";
        public static string DaybatchUrl => $"{CatiBaseUrl}/Blaise/Daybatch";
        public static string SchedulerUrl => $"{CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}";
        public static string SpecificationUrl => $"{CatiBaseUrl}/Blaise/Specification";
        public static string SurveyUrl => $"{CatiBaseUrl}/Blaise";
        public static string CaseUrl => $"{CatiBaseUrl}/Blaise/CaseInfo/StartSurvey?url={CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}/&rp.KeyValue=";
        public static string CaseInfoUrl => $"{CatiBaseUrl}/Blaise/CaseInfo";
        public static string NewDashboardCaseInfoUrl => $"{CatiBaseUrl}/BlaiseDashboard/Cati/CaseInfo";
    }
}
