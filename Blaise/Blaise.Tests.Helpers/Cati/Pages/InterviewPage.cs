using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class InterviewPage : BasePage
    {
        private readonly string _caseIdPath = "//div[contains(text(), 'Case:')]";

        public InterviewPage() : base(CatiConfigurationHelper.InterviewUrl)
        {
        }

        public string GetCaseIdText()
        {
            return GetElementTextByPath(_caseIdPath);
        }
    }
}
