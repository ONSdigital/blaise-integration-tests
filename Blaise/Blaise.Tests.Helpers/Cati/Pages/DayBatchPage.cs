using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class DayBatchPage : BasePage
    {
        private readonly string _dayBatchCreateButtonId = "btnCreateDaybatch";
        private readonly string _createButtonPath = "//input[@value='Create']";
        private readonly string _numberOfEntriesPath = "//div[contains(text(), 'Showing')]";

        public DayBatchPage(IWebDriver driver) : base(driver, CatiConfigurationHelper.DayBatchUrl)
        {
        }

        public void CreateDayBatch()
        {
            ClickButtonById(_dayBatchCreateButtonId);
            ClickButtonByXPath(_createButtonPath);
        }

        public string GetDaybatchEntriesText()
        {
            return GetElementTextByPath(_numberOfEntriesPath);
        }
    }
}
