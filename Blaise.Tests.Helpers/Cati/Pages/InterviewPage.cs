using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class InterviewPage : BasePage
    {
        private const string CaseIdPath = "//div[contains(text(), 'Case:')]";
        private const string FirstFocusId = "firstFocusable";

        public InterviewPage() : base(CatiConfigurationHelper.InterviewUrl)
        {
        }

        public string GetCaseIdText()
        {
            BrowserHelper.WaitForElement(By.XPath(CaseIdPath));
            return GetElementTextByPath(CaseIdPath);
        }

        public void WaitForFirstFocusObject()
        {
            GetElementTextById(FirstFocusId);
        }
    }
}
