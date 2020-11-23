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

        private By dayBatchCreateButton = By.Id("btnCreateDaybatch");
        private By createButton = By.XPath("//input[@value='Create']");
        private By numberOfEnteries = By.XPath("//div[contains(text(), 'Showing')]");

        public DayBatchPage(IWebDriver driver)
        {
            this._driver = driver;
        }

        public string CreateDayBatch()
        {
            _driver.FindElement(dayBatchCreateButton).Click();
            Thread.Sleep(2000);
            _driver.FindElement(createButton).Click();
            Thread.Sleep(2000);
            return _driver.FindElement(numberOfEnteries).Text;
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
