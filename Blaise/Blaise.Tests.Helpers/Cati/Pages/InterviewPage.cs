using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class InterviewPage : BasePage
    {
        private readonly string _saveContinueButtonId = "q";

        public InterviewPage(IWebDriver driver) : base(driver, CatiConfigurationHelper.InterviewUrl)
        {
        }

        public string GetSaveAndContinueButton()
        {
            return GetElementTextById(_saveContinueButtonId);
        }
    }
}
