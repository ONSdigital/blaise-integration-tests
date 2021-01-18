using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class DayBatchPage : BasePage
    {
        private const string DayBatchCreateButtonId = "btnCreateDaybatch";
        private const string CreateButtonPath = "//input[@value='Create']";
        private const string NumberOfEntriesPath = "//div[contains(text(), 'Showing')]";

        public DayBatchPage() : base(CatiConfigurationHelper.DayBatchUrl)
        {
        }

        public void CreateDayBatch()
        {
            ClickButtonById(DayBatchCreateButtonId);
            ClickButtonByXPath(CreateButtonPath);
        }

        public string GetDaybatchEntriesText()
        {
            return GetElementTextByPath(NumberOfEntriesPath);
        }
    }
}
