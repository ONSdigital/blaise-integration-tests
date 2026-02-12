namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
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

        private bool UseNewSelectors
        {
            get
            {
                try
                {
                    return BrowserHelper.ElementExistsByXPath("//i[contains(@class, 'bi-bell-fill')]", TimeSpan.FromSeconds(1));
                }
                catch
                {
                    return false;
                }
            }
        }

        private string CreateButtonSelector => UseNewSelectors
            ? "qa_btn_submit"
            : "//input[@value='Create']";

        private string StartTimeId => UseNewSelectors
            ? "qa_starttime"
            : "NewStartTimeAmPm";

        private string EndTimeId => UseNewSelectors
            ? "qa_endtime"
            : "NewEndTimeAmPm";

        private string UpdateButtonSelector => UseNewSelectors
            ? "qa_btn_submit"
            : "//input[@value='Update']";

        private string DaybatchEntryPath => UseNewSelectors
            ? $"//table[@id='Daybatch_content_table']//td[contains(., '{BlaiseConfigurationHelper.QuestionnaireName}')]"
            : $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']";

        private string ModifyEntrySelector => UseNewSelectors
            ? "qa_editrecord_0"
            : $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']/a";

        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        public DaybatchPage()
            : base(CatiConfigurationHelper.DaybatchUrl)
        {
        }

        public void CreateDaybatch()
        {
            ClickButtonById(DaybatchCreateButtonId);
            SelectDropDownValueById(QuestionnaireDropDownId, BlaiseConfigurationHelper.QuestionnaireName);

            if (UseNewSelectors)
            {
                BrowserHelper.ClickByIdWithRetry(CreateButtonSelector);
            }
            else
            {
                BrowserHelper.ClickByXPathWithRetry(CreateButtonSelector);
            }
        }

        public string GetDaybatchEntriesText()
        {
            return GetElementTextByPath(DaybatchEntryPath);
        }

        public void ApplyFilter()
        {
            if (UseNewSelectors)
            {
                ClickButtonByXPath("//div[@e-mappinguid='qa_instrumentid' and contains(@class, 'e-filtermenudiv')]");
                var dropdownSelector = "//span[contains(@class, 'e-ddl') and .//input[@id='qa_instrumentnameidfilter']]";
                ClickButtonByXPath(dropdownSelector);
                var listOptionPath = $"//li[contains(@class, 'e-list-item') and text()='{BlaiseConfigurationHelper.QuestionnaireName}']";
                ClickButtonByXPath(listOptionPath);
                ClickButtonByXPath("//button[contains(@class, 'e-flmenu-okbtn') and text()='Filter']");
                Thread.Sleep(1000);
            }
            else
            {
                ClickButtonByXPath(FilterButton);
                var filterButtonText = GetElementTextByPath(FilterButton);
                if (filterButtonText != "Filters (active)")
                {
                    ClickButtonByXPath(_surveyRadioButton);
                    ClickButtonByXPath(ApplyButton);
                }
                ClickButtonByXPath(FilterButton);
            }
        }

        internal void ModifyDaybatchEntry()
        {
            if (UseNewSelectors)
            {
                BrowserHelper.ScrollIntoViewAndClickById(ModifyEntrySelector);
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
