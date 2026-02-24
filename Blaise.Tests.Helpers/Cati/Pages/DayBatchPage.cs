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
                ClickButtonByXPath("//div[contains(@class, 'e-filtermenudiv')]");
                PopulateInputById("Daybatch_SearchBox", "DST2304Z");
                ClickButtonById("qa_instrumentid_excelDlg");
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
            Console.WriteLine("Starting navigation to the Daybatch page...");

            for (int attempt = 0; attempt < 3; attempt++)
            {
                try
                {
                    if (UseNewSelectors)
                    {
                        Console.WriteLine("Using new selectors. Navigating to the new dashboard Daybatch URL.");
                        BrowserHelper.NavigateToPage(CatiConfigurationHelper.NewDashboardDaybatchUrl);
                    }
                    else
                    {
                        Console.WriteLine("Using old selectors. Navigating to the old dashboard Daybatch URL.");
                        BrowserHelper.NavigateToPage(CatiConfigurationHelper.DaybatchUrl);
                    }

                    // Log the current URL after navigation
                    var currentUrl = BrowserHelper.GetCurrentUrl();
                    Console.WriteLine($"Attempt {attempt + 1}: Navigated to URL: {currentUrl}");

                    // Check if stuck on the Surveys page
                    if (currentUrl.Contains("Survey"))
                    {
                        Console.WriteLine("Redirected to the survey page. Attempting to navigate back to the Daybatch page...");

                        // Force navigation back to the Daybatch page
                        BrowserHelper.NavigateToPage(UseNewSelectors
                            ? CatiConfigurationHelper.NewDashboardDaybatchUrl
                            : CatiConfigurationHelper.DaybatchUrl);

                        Thread.Sleep(2000); // Wait before retrying
                        continue;
                    }

                    // Validate the current URL explicitly
                    if (currentUrl.Contains("Daybatch"))
                    {
                        Console.WriteLine("Successfully navigated to the Daybatch page.");

                        // Wait for the Daybatch table to load
                        if (BrowserHelper.ElementExistsByXPath("//*[@id='Daybatch_content_table']", TimeSpan.FromSeconds(30)))
                        {
                            Console.WriteLine("Daybatch table loaded successfully.");
                            return; // Successfully navigated and table loaded
                        }

                        Console.WriteLine("Daybatch table did not load. Retrying...");
                    }
                    else
                    {
                        Console.WriteLine("Unexpected URL. Retrying navigation...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during navigation attempt {attempt + 1}: {ex.Message}");
                }

                Thread.Sleep(2000); // Wait before retrying
            }

            throw new Exception("Failed to navigate to the Daybatch page after multiple attempts. Ensure the URL and page structure are correct.");
        }

        // Added a public property to expose the UseNewSelectors logic
        public bool IsUsingNewSelectors => UseNewSelectors;
    }
}
