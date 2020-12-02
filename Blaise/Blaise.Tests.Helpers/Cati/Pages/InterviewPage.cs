using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class InterviewPage : BasePage
    {
        private const string CaseIdPath = "//div[contains(text(), 'Case:')]";

        public InterviewPage() : base(CatiConfigurationHelper.InterviewUrl)
        {
        }

        public string GetCaseIdText()
        {
            return GetElementTextByPath(CaseIdPath);
        }
    }
}
