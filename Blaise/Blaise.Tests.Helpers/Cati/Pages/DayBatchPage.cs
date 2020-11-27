using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class DayBatchPage
    {
        private readonly By _dayBatchCreateButton = By.Id("btnCreateDaybatch");
        private readonly By _createButton = By.XPath("//input[@value='Create']");
        private readonly By _numberOfEntries = By.XPath("//div[contains(text(), 'Showing')]");

        private readonly IWebDriver _driver;

        public DayBatchPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void CreateDayBatch()
        {
            _driver.FindElement(_dayBatchCreateButton).Click();
            Thread.Sleep(2000);

            _driver.FindElement(_createButton).Click();
            Thread.Sleep(2000);      
        }

        public string GetDaybatchEntriesText()
        {
            return _driver.FindElement(_numberOfEntries).Text;
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(CatiConfigurationHelper.DayBatchUrl);
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
