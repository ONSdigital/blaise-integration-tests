namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System.Threading;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;

    public class DaybatchPage : BasePage
    {
        private const string DaybatchCreateButtonId = "btnCreateDaybatch";
        private const string QuestionnaireDropDownId = "InstrumentId";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        public DaybatchPage()
            : base(CatiConfigurationHelper.DaybatchUrl)
        {
        }

        private bool UseNewSelectors
        {
            get
            {
                try
                {
                    return BrowserHelper.ElementExistsByXPath("//i[contains(@class, 'bi-bell-fill')]");
                }
                catch
                {
                    return false;
                }
            }
        }

        // Dynamic Selectors
        private string CreateButtonSelector => UseNewSelectors
            ? "qa_btn_submit" // V16 ID
            : "//input[@value='Create']"; // V14 XPath

        private string StartTimeId => UseNewSelectors
            ? "qa_starttime"
            : "NewStartTimeAmPm";

        private string EndTimeId => UseNewSelectors
            ? "qa_endtime"
            : "NewEndTimeAmPm";

        private string UpdateButtonSelector => UseNewSelectors
            ? "qa_btn_submit" // V16 ID
            : "//input[@value='Update']"; // V14 XPath

        private string DayBatchEntryPath => UseNewSelectors
            ? $"//table[@id='Daybatch_content_table']//td[contains(., '{BlaiseConfigurationHelper.QuestionnaireName}')]"
            : $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']";

        private string ModifyEntrySelector => UseNewSelectors
            ? "qa_editrecord_0" // V16 ID
            : $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']/a"; // V14 XPath

        public void CreateDaybatch()
        {
            ClickButtonById(DaybatchCreateButtonId);
            Thread.Sleep(2000);
            SelectDropDownValueById(QuestionnaireDropDownId, BlaiseConfigurationHelper.QuestionnaireName);
            Thread.Sleep(3000);

            if (UseNewSelectors)
            {
                ClickButtonById(CreateButtonSelector);
            }
            else
            {
                ClickButtonByXPath(CreateButtonSelector);
            }
        }

        public string GetDaybatchEntriesText()
        {
            return GetElementTextByPath(DayBatchEntryPath);
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
            if (UseNewSelectors)
            {
                ClickButtonById(ModifyEntrySelector);
            }
            else
            {
                ClickButtonByXPath(ModifyEntrySelector);
            }

            PopulateInputById(StartTimeId, "12:00 AM");
            PopulateInputById(EndTimeId, "11:59 PM");

            if (UseNewSelectors)
            {
                ClickButtonById(UpdateButtonSelector);
            }
            else
            {
                ClickButtonByXPath(UpdateButtonSelector);
            }
        }
    }
}
