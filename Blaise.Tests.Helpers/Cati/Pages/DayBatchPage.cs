using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using System.Threading;
// ReSharper disable InconsistentNaming

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class DayBatchPage : BasePage
    {
        private const string DayBatchCreateButtonId = "btnCreateDaybatch";
        private const string CreateButtonPath = "//input[@value='Create']";
        private readonly string DayBatchEntry = $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']";
        private readonly string ModifyEntryPath = $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']/a";
        private const string StartTimeId = "NewStartTimeAmPm";
        private const string EndTimeId = "NewEndTimeAmPm";
        private const string UpdateButtonPath = "//input[@value='Update']";
        private const string InstrumentDropDownId = "InstrumentId";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private readonly string SurveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";


        public DayBatchPage() : base(CatiConfigurationHelper.DayBatchUrl)
        {
        }

        public void CreateDayBatch()
        {
            ClickButtonById(DayBatchCreateButtonId);
            Thread.Sleep(2000);
            SelectDropDownValueById(InstrumentDropDownId, BlaiseConfigurationHelper.QuestionnaireName);
            Thread.Sleep(3000);
            ClickButtonByXPath(CreateButtonPath);
        }

        public string GetDaybatchEntriesText()
        {
            return GetElementTextByPath(DayBatchEntry);
        }

        internal void ModifyDayBatchEntry()
        {
            ClickButtonByXPath(ModifyEntryPath);
            PopulateInputById(StartTimeId, "12:00 AM");
            PopulateInputById(EndTimeId, "11:59 PM");
            ClickButtonByXPath(UpdateButtonPath);
        }

        public void ApplyFilters()
        {
            Thread.Sleep(5000);
            ClickButtonByXPath(FilterButton);
            var filterButtonText = GetElementTextByPath(FilterButton);
            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(SurveyRadioButton);
                ClickButtonByXPath(ApplyButton);
            }
        }
    }
}
