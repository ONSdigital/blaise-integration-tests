using System;
using Blaise.Tests.Helpers.Configuration.I;
using Blaise.Tests.Helpers.Configuration.Interfaces;
using Blaise.Tests.Helpers.Framework.Extensions;

namespace Blaise.Tests.Helpers.Configuration
{
    public class CatiConfigurationHelper : ICatiConfigurationHelper
    {
        private readonly Guid _adminPassword = Guid.NewGuid();
        private readonly Guid _interviewerPassword = Guid.NewGuid();

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
                var baseUrl = ConfigurationExtensions.GetVariable("ENV_BLAISE_CATI_URL");
                return baseUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    ? baseUrl
                    : "https://" + baseUrl;
            }
        }

        public string LoginUrl => $"{CatiBaseUrl}/blaise/account/login";
        public string DayBatchUrl => $"{CatiBaseUrl}/blaise/daybatch";
        public string SchedulerUrl => $"{CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}";
        public string SpecificationUrl => $"{CatiBaseUrl}/blaise/specification";
        public string SurveyUrl => $"{CatiBaseUrl}/blaise";
        public string CaseUrl => $"{CatiBaseUrl}//Blaise/CaseInfo/StartSurvey?url={CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}/&rp.KeyValue=";
        public string CaseInfoUrl => $"{CatiBaseUrl}/blaise/CaseInfo";
    }

}
