namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class SurveyPage : BasePage
    {
        private const string ClearCatiDataButtonPath = @"//*[@id='MVCGridTable_SurveysGrid']/tbody/tr/td[9]/a";
        private const string BackupDataButtonId = "chkBackupAll";
        private const string ClearDataButtonId = "chkClearAll";
        private const string ExecuteButtonPath = "//input[@value='Execute']";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        public SurveyPage()
            : base(CatiConfigurationHelper.SurveyUrl)
        {
        }

        protected override By PageIdentityBy => UseNewSelectors
            ? By.XPath("//div[contains(@class,'e-filtermenudiv') and (@e-mappinguid='qa_instrumentid' or @e-mappinguid='qa_instrument')]")
            : By.XPath(FilterButton);

        private bool UseNewSelectors
        {
            get
            {
                return CatiUiVersionHelper.IsNewUi;
            }
        }

        public void ClearDaybatchEntries()
        {
            Console.WriteLine("Starting: Clear daybatch entries.");

            if (UseNewSelectors)
            {
                Console.WriteLine("Using new selectors to clear daybatch entries.");
                var downloadButtonPath = $"//tr[contains(., '{BlaiseConfigurationHelper.QuestionnaireName}')]//span[contains(@class, 'bi-download')]";
                ClickButtonByXPath(downloadButtonPath);
                Console.WriteLine("Clicked download button.");
                ClickButtonByXPath("//label[@for='qa_backup_all']");
                Console.WriteLine("Selected backup all option.");
                ClickButtonByXPath("//label[@for='qa_clear_all']");
                Console.WriteLine("Selected clear all option.");
                BrowserHelper.ClickByIdWithRetry("qa_btn_submit");
                Console.WriteLine("Submitted clear daybatch entries.");
            }
            else
            {
                Console.WriteLine("Using old selectors to clear daybatch entries.");
                ClickButtonByXPath(ClearCatiDataButtonPath);
                Console.WriteLine("Clicked clear CATI data button.");
                ClickButtonById(BackupDataButtonId);
                Console.WriteLine("Clicked backup data button.");
                ClickButtonById(ClearDataButtonId);
                Console.WriteLine("Clicked clear data button.");
                ClickButtonByXPath(ExecuteButtonPath);
                Console.WriteLine("Executed clear daybatch entries.");
            }
        }

        public void ApplyFilter()
        {
            Console.WriteLine("Starting: Apply filter.");
            if (UseNewSelectors)
            {
                SyncfusionGridFilterHelper.ApplyNewUiFilterWithRetry(BlaiseConfigurationHelper.QuestionnaireName);
                Console.WriteLine("Filtered Questionnaire.");
            }
            else
            {
                Console.WriteLine("Using old selectors to apply filter.");
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

        public void WaitForSurveyTable()
        {
            if (UseNewSelectors)
            {
                BrowserHelper.WaitUntilGridHasLoadedData();
            }
            else
            {
                BrowserHelper.WaitForElementByXPath("//*[@id='MVCGridTable_SurveysGrid']");
            }
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            var baseLoaded = base.PageHasLoaded();
            return driver => baseLoaded(driver) &&
                (UseNewSelectors
                    ? BodyDoesNotContainText("No records to display")(driver)
                    : BodyContainsText("Showing")(driver));
        }
    }
}
