using System.Configuration;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class CatiConfigurationHelper
    {
        public static string CatiAdminUsername => GetVariable("CatiAdminUsername");
        public static string CatiAdminPassword => GetVariable("CatiAdminPassword");
        public static string CatiInterviewUsername => GetVariable("CatiInterviewUsername");
        public static string CatiInterviewPassword => GetVariable("CatiInterviewPassword");
        public static string LoginUrl => GetVariable("LoginUrl");
        public static string CaseInfoUrl => GetVariable("CaseInfoUrl");
        public static string DayBatchUrl => GetVariable("DayBatchUrl");
        public static string InterviewUrl => GetVariable("InterviewUrl");
        public static string SpecificationUrl => GetVariable("SpecificationUrl");
        public static string SurveyUrl => GetVariable("SurveyUrl");
        public static string ChromeDriver => GetVariable("ChromeDriver");
        public static string InterviewRole => GetVariable("InterviewRole");

        private static string GetVariable(string variableName)
        {
            var value = ConfigurationManager.AppSettings[variableName];
            return value;
        }
    }
}
