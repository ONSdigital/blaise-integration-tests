using System;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class CatiConfigurationHelper
    {
        public static string CatiAdminUsername => BlaiseConfigurationHelper.BuildConnectionModel().UserName;
        public static string CatiAdminPassword => BlaiseConfigurationHelper.BuildConnectionModel().Password;
        public static string CatiInterviewUsername => "DSTTestUser";
        public static string CatiInterviewPassword => $"{Guid.NewGuid()}";
        public static string InterviewRole => $"DST";
        public static string CatiBaseUrl => $"https://{BlaiseConfigurationHelper.BuildConnectionModel().ServerName.Replace("client", "web")}";
        public static string LoginUrl => $"{CatiBaseUrl}/blaise/account/login";
        public static string DayBatchUrl => $"{CatiBaseUrl}/blaise/daybatch";
        public static string InterviewUrl => $"{CatiBaseUrl}/{BlaiseConfigurationHelper.InstrumentName}/";
        public static string SpecificationUrl => $"{CatiBaseUrl}/blaise/specification";
        public static string SurveyUrl => $"{CatiBaseUrl}/blaise/";
    }
}