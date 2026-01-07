using System;
using Blaise.Tests.Helpers.Configuration.Interfaces;
using Blaise.Tests.Helpers.Framework.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public class CatiConfigurationHelperV16 : ICatiConfigurationHelper
    {
        private readonly Guid _adminPassword;
        private readonly Guid _interviewerPassword;

        public CatiConfigurationHelperV16()
        {
            _adminPassword = Guid.NewGuid();
            _interviewerPassword = Guid.NewGuid();
        }

        public string CatiAdminUsername => "DSTAdminUser";
        public string CatiAdminPassword => _adminPassword.ToString();
        public string AdminRole => "DST";

        public string CatiInterviewUsername => "DSTTestUser";
        public string CatiInterviewPassword => _interviewerPassword.ToString();
        public string InterviewRole => "DST";

        public string CatiBaseUrl
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

        public string LoginUrl => $"{CatiBaseUrl}BlaiseDashboard/account/login";
        public string DayBatchUrl => $"{CatiBaseUrl}BlaiseDashboard/daybatch";
        public string SchedulerUrl => $"{CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}";
        public string SpecificationUrl => $"{CatiBaseUrl}/BlaiseDashboard/specification";
        public string SurveyUrl => $"{CatiBaseUrl}BlaiseDashboard";
        public string CaseUrl => $"{CatiBaseUrl}//BlaiseDashboard/CaseInfo/StartSurvey?url={CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}/&rp.KeyValue=";
        public string CaseInfoUrl => $"{CatiBaseUrl}BlaiseDashboard/CaseInfo";
    }
}
