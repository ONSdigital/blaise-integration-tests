using System;
using System.Linq;

namespace Blaise.Tests.Helpers.Configuration
{
    public static class CatiConfigurationHelper
    {
        private static readonly Guid Password;
        private static readonly string UserName;

        static CatiConfigurationHelper()
        {
            UserName = "DSTTestUser" + RandomString(2);
            Password = Guid.NewGuid();
        }

        public static string CatiAdminUsername => BlaiseConfigurationHelper.BuildConnectionModel().UserName;
        public static string CatiAdminPassword => BlaiseConfigurationHelper.BuildConnectionModel().Password;
        public static string CatiInterviewUsername => $"{UserName}";
        public static string CatiInterviewPassword => $"{Password}";
        public static string InterviewRole => $"DST";
        public static string CatiBaseUrl => $"{BlaiseConfigurationHelper.BuildConnectionModel().Binding}://{BlaiseConfigurationHelper.BuildConnectionModel().ServerName.Replace("client", "web")}";
        public static string LoginUrl => $"{CatiBaseUrl}/blaise/account/login";
        public static string DayBatchUrl => $"{CatiBaseUrl}/blaise/daybatch";
        public static string InterviewUrl => $"{CatiBaseUrl}/{BlaiseConfigurationHelper.InstrumentName}/";
        public static string SpecificationUrl => $"{CatiBaseUrl}/blaise/specification";
        public static string SurveyUrl => $"{CatiBaseUrl}/blaise/";

        private static readonly Random random = new Random((int)DateTime.Now.Ticks);

        private static string RandomString(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
    }
}