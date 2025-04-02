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
        private readonly string DayBatchEntry = $"//div[contains(text(), '{BlaiseConfigurationHelper.QuestionnaireName}')]";
        private readonly string ModifyEntryPath = $"/html/body/div[1]/main/div[2]/div/div[5]/div/table/tbody/tr/td[18]/div/div/button[1]/span";
        private const string StartTimeId = "qa_starttime";
        private const string EndTimeId = "qa_endtime";
        private const string UpdateButtonId = "qa_btn_submit";
        private const string QuestionnaireDropDownId = "InstrumentId";
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
            SelectDropDownValueById(QuestionnaireDropDownId, BlaiseConfigurationHelper.QuestionnaireName);
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
            ClickButtonById(UpdateButtonId);
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
