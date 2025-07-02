using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class InterviewPage : BasePage
    {
        private const string _caseIdPath = "//div[contains(text(), 'Case:')]";
        private const string _firstFocusId = "firstFocusable";

        public InterviewPage()
            : base(CatiConfigurationHelper.SchedulerUrl, "LayoutSet=CATI-Interviewer_Large")
        {
        }

        public string GetCaseIdText()
        {
            return GetElementTextByPath(_caseIdPath);
        }

        public void WaitForFirstFocusObject()
        {
            GetElementTextById(_firstFocusId);
        }
    }
}
