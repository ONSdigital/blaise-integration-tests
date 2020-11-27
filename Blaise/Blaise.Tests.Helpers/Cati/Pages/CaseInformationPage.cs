using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class CaseInformationPage
    {
        private readonly By _loadCaseButton = By.XPath("//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a");

        private readonly IWebDriver _driver;
        public CaseInformationPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void LoadCase()
        {
            Thread.Sleep(1000);

            _driver.FindElement(_loadCaseButton).Click();
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(CatiConfigurationHelper.CaseInfoUrl);
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
