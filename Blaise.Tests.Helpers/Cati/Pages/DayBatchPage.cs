using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using System;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class DayBatchPage : BasePage
    {
        private const string DayBatchCreateButtonId = "btnCreateDaybatch";
        private const string CreateButtonPath = "//input[@value='Create']";
        private const string NumberOfEntriesPath = "//div[contains(text(), 'Showing')]";
        private const string ModifyEntryPath = "//*[@id='MVCGridTable_DaybatchGrid']/tbody/tr/td[17]/a";
        private const string StartTimeId = "NewStartTimeAmPm";
        private const string EndTimeId = "NewEndTimeAmPm";
        private const string UpdateButtonPath = "//input[@value='Update']";

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

        internal void ModifyDayBatchEntry()
        {
            ClickButtonByXPath(ModifyEntryPath);
            PopulateInputById(StartTimeId, "12:00 AM");
            PopulateInputById(EndTimeId, "11:59 PM");
            ClickButtonByXPath(UpdateButtonPath);
        }
    }
}
