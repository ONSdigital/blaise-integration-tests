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
        private IWebDriver _driver;

        private By loadCaseButton = By.XPath("//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a");

        public CaseInfoPage(IWebDriver driver)
        {
            this._driver = driver;
        }

        public void loadCase()
        {
            Thread.Sleep(1000);
            _driver.FindElement(loadCaseButton).Click();
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl("");
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
