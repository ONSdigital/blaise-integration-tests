using System.Threading;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;

// ReSharper disable InconsistentNaming
namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class DayBatchPage : BasePage
    {
        private const string _dayBatchCreateButtonId = "btnCreateDaybatch";
        private const string _createButtonPath = "//input[@value='Create']";
        private readonly string _dayBatchEntry = $"//table[@id='Daybatch_content_table']//td[contains(., '{BlaiseConfigurationHelper.QuestionnaireName}')]";
        private readonly string _modifyEntryButtonId = "qa_editrecord_0";
        private const string _startTimeId = "qa_starttime";
        private const string _endTimeId = "qa_endtime";
        private const string _updateButtonId = "qa_btn_submit";
        private const string _questionnaireDropDownId = "InstrumentId";
        private const string _filterButton = "//*[contains(text(), 'Filters')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";
        private const string _applyButton = "//*[contains(text(), 'Apply')]";

        private static IWebDriver _browser;


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
            ClickButtonById(_modifyEntryButtonId);
            PopulateInputById(_startTimeId, "12:00 AM");
            PopulateInputById(_endTimeId, "11:59 PM");
            ClickButtonById(_updateButtonId);
        }

        public void ApplyFilters()
        {
            Thread.Sleep(5000);
            if (!BrowserHelper.ElementExistsByXPath(_applyButton))
            {
                ClickButtonByXPath(_filterButton);
            }

            ClickButtonByXPath(_surveyRadioButton);
            ClickButtonByXPath(_applyButton);
        }
    }
}
