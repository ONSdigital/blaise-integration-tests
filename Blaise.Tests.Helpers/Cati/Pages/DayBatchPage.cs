namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System.Threading;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;

    public class DaybatchPage : BasePage
    {
        private const string DaybatchCreateButtonId = "btnCreateDaybatch";
        private const string CreateButtonPath = "//input[@value='Create']";
        private const string StartTimeId = "NewStartTimeAmPm";
        private const string EndTimeId = "NewEndTimeAmPm";
        private const string UpdateButtonPath = "//input[@value='Update']";
        private const string QuestionnaireDropDownId = "InstrumentId";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";
        private readonly string _dayBatchEntry = $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']";
        private readonly string _modifyEntryPath = $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']/a";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        public DaybatchPage()
            : base(CatiConfigurationHelper.DaybatchUrl)
        {
        }

        public void CreateDaybatch()
        {
            ClickButtonById(DaybatchCreateButtonId);
            Thread.Sleep(2000);
            SelectDropDownValueById(QuestionnaireDropDownId, BlaiseConfigurationHelper.QuestionnaireName);
            Thread.Sleep(3000);
            ClickButtonByXPath(CreateButtonPath);
        }

        public string GetDaybatchEntriesText()
        {
            return GetElementTextByPath(_dayBatchEntry);
        }

        public void ApplyFilters()
        {
            Thread.Sleep(5000);
            ClickButtonByXPath(FilterButton);
            var filterButtonText = GetElementTextByPath(FilterButton);
            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(_surveyRadioButton);
                ClickButtonByXPath(ApplyButton);
            }
        }

        internal void ModifyDaybatchEntry()
        {
            ClickButtonByXPath(_modifyEntryPath);
            PopulateInputById(StartTimeId, "12:00 AM");
            PopulateInputById(EndTimeId, "11:59 PM");
            ClickButtonByXPath(UpdateButtonPath);
        }
    }
}
