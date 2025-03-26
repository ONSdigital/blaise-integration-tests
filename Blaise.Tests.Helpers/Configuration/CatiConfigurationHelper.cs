﻿using Blaise.Tests.Helpers.Framework.Extensions;
using System;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class CatiConfigurationHelper
    {
        private static readonly Guid AdminPassword;
        private static readonly Guid InterviewerPassword;

        static CatiConfigurationHelper()
        {
            AdminPassword = Guid.NewGuid();
            InterviewerPassword = Guid.NewGuid();
        }

        public static string CatiAdminUsername => "DSTAdminUser";
        public static string CatiAdminPassword => $"{AdminPassword}";
        public static string AdminRole => $"DST";
        public static string CatiInterviewUsername => "DSTTestUser";
        public static string CatiInterviewPassword => $"{InterviewerPassword}";
        public static string InterviewRole => $"DST";
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
        public static string LoginUrl => $"{CatiBaseUrl}/blaisedashboard/account/login";
        public static string DayBatchUrl => $"{CatiBaseUrl}/blaisedashboard/daybatch";
        public static string SchedulerUrl => $"{CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}";
        public static string SpecificationUrl => $"{CatiBaseUrl}/blaisedashboard/specification";
        public static string SurveyUrl => $"{CatiBaseUrl}/blaisedashboard";
        public static string CaseUrl => $"{CatiBaseUrl}//BlaiseDashboard/CaseInfo/StartSurvey?url={CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}/&rp.KeyValue=";
        public static string CaseInfoUrl => $"{CatiBaseUrl}/blaisedashboard/CaseInfo";
    }
}
