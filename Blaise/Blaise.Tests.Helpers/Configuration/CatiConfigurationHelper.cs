using System.Configuration;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class CatiConfigurationHelper
    {
        public static string CatiUsername => GetVariable("CatiUsername");
        public static string CatiPassword => GetVariable("CatiPassword");
        public static string LoginUrl => GetVariable("LoginUrl");
        public static string CaseInfoUrl => GetVariable("CaseInfoUrl");
        public static string DayBatchUrl => GetVariable("DayBatchUrl");
        public static string InterviewUrl => GetVariable("InterviewUrl");
        public static string SpecificationUrl => GetVariable("SpecificationUrl");
        public static string SurveyUrl => GetVariable("SurveyUrl");
        public static string ChromeDriver => GetVariable("ChromeDriver");

        private static string GetVariable(string variableName)
        {
            var value = ConfigurationManager.AppSettings[variableName];
            return value;
        }
    }
}
