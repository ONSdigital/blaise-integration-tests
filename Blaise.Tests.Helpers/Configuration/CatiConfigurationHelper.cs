using Blaise.Tests.Helpers.Framework.Extensions;
using System;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class CatiConfigurationHelper
    {
        private static readonly Guid Password;

        static CatiConfigurationHelper()
        {
            Password = Guid.NewGuid();
        }

        public static string CatiAdminUsername => BlaiseConfigurationHelper.BuildConnectionModel().UserName;
        public static string CatiAdminPassword => BlaiseConfigurationHelper.BuildConnectionModel().Password;
        public static string CatiInterviewUsername => "DSTTestUser";
        public static string CatiInterviewPassword => $"{Password}";
        public static string InterviewRole => $"DST";
        public static string CatiBaseUrl
        {
            get
            {
                string baseUrl = ConfigurationExtensions.GetVariable("ENV_BLAISE_CATI_URL");
                if (!baseUrl.StartsWith("http://") && !baseUrl.StartsWith("https://"))
                {
                    baseUrl = "http://" + baseUrl;
                }

                return baseUrl;
            }
        }
        public static string LoginUrl => $"{CatiBaseUrl}/blaise/account/login";
        public static string DayBatchUrl => $"{CatiBaseUrl}/blaise/daybatch";
        public static string InterviewUrl => $"{CatiBaseUrl}/{BlaiseConfigurationHelper.InstrumentName}";
        public static string SpecificationUrl => $"{CatiBaseUrl}/blaise/specification";
        public static string SurveyUrl => $"{CatiBaseUrl}/blaise";
        public static string CaseUrl => $"{CatiBaseUrl}//Blaise/CaseInfo/StartSurvey?url={CatiBaseUrl}/{BlaiseConfigurationHelper.InstrumentName}/&rp.KeyValue=";
        public static string CaseInfoUrl => $"{CatiBaseUrl}/blaise/CaseInfo";
    }
}