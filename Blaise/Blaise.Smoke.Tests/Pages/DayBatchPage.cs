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
    class DayBatchPage
    {
        private IWebDriver _driver;
        private ConfigurationHelper _configurationHelper;

        private By dayBatchCreateButton = By.Id("btnCreateDaybatch");
        private By createButton = By.XPath("//input[@value='Create']");
        private By numberOfEnteries = By.XPath("//div[contains(text(), 'Showing')]");

        public DayBatchPage(IWebDriver driver)
        {
            this._driver = driver;
            this._configurationHelper = new ConfigurationHelper();
        }

        public void CreateDayBatch()
        {
            _driver.FindElement(dayBatchCreateButton).Click();
            Thread.Sleep(2000);
            _driver.FindElement(createButton).Click();
            Thread.Sleep(2000);      
        }

        public string CheckEnteriesInDayBatch()
        {
            return _driver.FindElement(numberOfEnteries).Text;
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(_configurationHelper.DayBatchURL);
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
