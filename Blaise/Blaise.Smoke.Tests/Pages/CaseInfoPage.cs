using Blaise.Smoke.Tests.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blaise.Smoke.Tests.Pages
{
    class CaseInfoPage
    {
        private readonly IWebDriver _driver;
        private readonly ConfigurationHelper _configurationHelper;

        private readonly By loadCaseButton = By.XPath("//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a");

        public CaseInfoPage(IWebDriver driver)
        {
            this._driver = driver;
            this._configurationHelper = new ConfigurationHelper();
        }

        public void LoadCase()
        {
            Thread.Sleep(1000);
            _driver.FindElement(loadCaseButton).Click();
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(_configurationHelper.CaseInfoURL);
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
