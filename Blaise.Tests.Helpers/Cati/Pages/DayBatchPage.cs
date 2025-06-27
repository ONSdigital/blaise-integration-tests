using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

// ReSharper disable InconsistentNaming
namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class DayBatchPage : BasePage
    {
        private const string _dayBatchCreateButtonId = "btnCreateDaybatch";
        private const string _createButtonPath = "//input[@value='Create']";
        private readonly string _dayBatchEntry = $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']";
        private readonly string _modifyEntryPath = $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']/a";
        private const string _startTimeId = "NewStartTimeAmPm";
        private const string _endTimeId = "NewEndTimeAmPm";
        private const string _updateButtonPath = "//input[@value='Update']";
        private const string _questionnaireDropDownId = "InstrumentId";
        private const string _filterButton = "//*[contains(text(), 'Filters')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";
        private const string _applyButton = "//*[contains(text(), 'Apply')]";

        public DayBatchPage()
            : base(CatiConfigurationHelper.DayBatchUrl)
        {
        }

        public void CreateDayBatch()
        {
            ClickButtonById(_dayBatchCreateButtonId);
            Thread.Sleep(2000);
            SelectDropDownValueById(_questionnaireDropDownId, BlaiseConfigurationHelper.QuestionnaireName);
            Thread.Sleep(3000);
            ClickButtonByXPath(_createButtonPath);
        }

        public string GetDaybatchEntriesText()
        {
            return GetElementTextByPath(_dayBatchEntry);
        }

        internal void ModifyDayBatchEntry()
        {
            ClickButtonByXPath(_modifyEntryPath);
            PopulateInputById(_startTimeId, "12:00 AM");
            PopulateInputById(_endTimeId, "11:59 PM");
            ClickButtonByXPath(_updateButtonPath);
        }

        public void ApplyFilters()
        {
            Thread.Sleep(5000);
            ClickButtonByXPath(_filterButton);
            var filterButtonText = GetElementTextByPath(_filterButton);
            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(_surveyRadioButton);
                ClickButtonByXPath(_applyButton);
            }
        }
    }
}
