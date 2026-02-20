namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Threading;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

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
                // Locate the table's scrollable container
                var tableScrollableContainer = BrowserHelper.FindElement(By.XPath("//*[@id='Daybatch_content_table']/parent::div"));

                // Locate the Modify Entry button
                var modifyEntryButton = BrowserHelper.FindElement(By.Id("qa_editrecord_0"));

                // Scroll the table horizontally to bring the Modify Entry button into view
                BrowserHelper.ExecuteJavaScript(
                    "arguments[0].scrollLeft = arguments[1].offsetLeft;",
                    tableScrollableContainer,
                    modifyEntryButton
                );

                // Click the Modify Entry button
                BrowserHelper.ScrollIntoViewAndClickById("qa_editrecord_0");

                // Set start time in the modal
                PopulateInputById("qa_starttime", ""); // Clear the input field first
                PopulateInputById("qa_starttime", "12:00 AM");

                // Set end time in the modal
                PopulateInputById("qa_endtime", ""); // Clear the input field first
                PopulateInputById("qa_endtime", "11:59 PM");

                // Click the update button
                ClickButtonById("qa_btn_submit");
            }
            else
            {
                ClickButtonByXPath(ModifyEntrySelector);

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

        public void NavigateToVersionSpecificPage()
        {
            if (UseNewSelectors)
            {
                BrowserHelper.NavigateToPage(CatiConfigurationHelper.NewDashboardDaybatchUrl);
            }
            else
            {
                BrowserHelper.NavigateToPage(CatiConfigurationHelper.DaybatchUrl);
            }

            // Log the current URL after navigation
            Console.WriteLine($"Navigated to URL: {BrowserHelper.GetCurrentUrl()}");

            // Wait for the Daybatch page to stabilize
            if (!BrowserHelper.ElementExistsByXPath("//*[@id='Daybatch_content_table']", TimeSpan.FromSeconds(30)))
            {
                throw new Exception("Daybatch table did not load within the expected time.");
            }

            // Check if the page redirects to the survey page
            if (BrowserHelper.GetCurrentUrl().Contains("Survey"))
            {
                throw new Exception("Unexpected redirection to the survey page.");
            }
        }

        // Added a public property to expose the UseNewSelectors logic
        public bool IsUsingNewSelectors => UseNewSelectors;
    }
}
