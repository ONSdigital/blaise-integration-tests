using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class InterviewPage
    {
        private readonly By _saveContinueButton = By.Id("q");

        private readonly IWebDriver _driver;
        public InterviewPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public string GetSaveAndContinueButton()
        {
            Thread.Sleep(3000);

            return _driver.FindElement(_saveContinueButton).Text;
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(CatiConfigurationHelper.InterviewUrl);
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
