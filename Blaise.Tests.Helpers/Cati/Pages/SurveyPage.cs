namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Linq;
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
        private const string InstrumentFilterIconXPath = "//div[contains(@class,'e-filtermenudiv') and @e-mappinguid='qa_instrumentid']";
        private const string FilterPopupXPath = "//div[contains(@class,'e-popup') and contains(@class,'e-popup-open')]";
        private const string InstrumentFilterPopupId = "qa_instrumentid-flmdlg";
        private const string FilterPopupButtonXPath = ".//button[contains(@class,'e-flmenu-okbtn') or normalize-space()='Filter' or normalize-space()='Apply']";
        private const string FilterPopupListItemXPath = "//div[contains(@class,'e-popup') and contains(@class,'e-popup-open')]//li[contains(@class,'e-list-item')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        public SurveyPage()
            : base(CatiConfigurationHelper.SurveyUrl)
        {
        }

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
                Console.WriteLine("Using new selectors to apply filter.");
                ClickButtonByXPath(InstrumentFilterIconXPath);
                Console.WriteLine("Opened filter menu.");
                BrowserHelper
                    .Wait("Timed out waiting for filter popup to open")
                    .Until(driver =>
                        driver.FindElements(By.Id(InstrumentFilterPopupId))
                            .Any(candidate => candidate.Displayed));
                var searchInput = BrowserHelper
                    .Wait("Timed out waiting for survey filter search input")
                    .Until(FindSurveySearchInput);
                searchInput.Clear();
                searchInput.SendKeys(BlaiseConfigurationHelper.QuestionnaireName);

                SelectSurveyFilterOption(BlaiseConfigurationHelper.QuestionnaireName);
                ConfirmSurveyFilterIfPossible();

                Console.WriteLine("Selected questionnaire from dropdown.");
                Console.WriteLine("Filtered Questionnaire.");
                BrowserHelper.WaitUntilGridHasLoadedData();
            }
            else
            {
                Console.WriteLine("Using old selectors to apply filter.");
                ClickButtonByXPath(FilterButton);
                Console.WriteLine("Clicked filter button.");
                var filterButtonText = GetElementTextByPath(FilterButton);
                if (filterButtonText != "Filters (active)")
                {
                    ClickButtonByXPath(_surveyRadioButton);
                    Console.WriteLine("Selected survey radio button.");
                    ClickButtonByXPath(ApplyButton);
                    Console.WriteLine("Clicked apply button.");
                }
                ClickButtonByXPath(FilterButton);
                Console.WriteLine("Closed filter menu.");
            }
        }

        private IWebElement FindSurveySearchInput(IWebDriver driver)
        {
            var activeElement = driver.SwitchTo().ActiveElement();
            if (activeElement != null &&
                activeElement.Displayed &&
                activeElement.TagName.Equals("input", StringComparison.OrdinalIgnoreCase))
            {
                return activeElement;
            }

            var popupById = driver.FindElements(By.Id(InstrumentFilterPopupId))
                .FirstOrDefault(candidate => candidate.Displayed);
            if (popupById != null)
            {
                var popupInputById = popupById
                    .FindElements(By.CssSelector("input[aria-label='multiselect'], input.e-multiselect, input[role='combobox']"))
                    .FirstOrDefault(candidate => candidate.Displayed);
                if (popupInputById != null)
                {
                    return popupInputById;
                }
            }

            var popup = driver.FindElements(By.XPath(FilterPopupXPath))
                .FirstOrDefault(candidate => candidate.Displayed);
            if (popup != null)
            {
                var popupInput = popup.FindElements(By.CssSelector("input[aria-label='multiselect'], input.e-multiselect, input[role='combobox'], input[type='text']"))
                    .FirstOrDefault(candidate => candidate.Displayed);
                if (popupInput != null)
                {
                    return popupInput;
                }
            }

            var byAria = driver.FindElements(By.CssSelector("input[aria-label='multiselect']"))
                .FirstOrDefault(candidate => candidate.Displayed);
            if (byAria != null)
            {
                return byAria;
            }

            return driver.FindElements(By.CssSelector("input[id^='multiselect-']"))
                .FirstOrDefault(candidate => candidate.Displayed);
        }

        private void SelectSurveyFilterOption(string questionnaireName)
        {
            var exactMatchXPath = $"//li[contains(@class,'e-list-item') and normalize-space()='{questionnaireName}']";
            var containsXPath = $"//li[contains(@class,'e-list-item') and contains(normalize-space(),'{questionnaireName}') ]";
            var roleOptionXPath = $"//li[@role='option' and contains(normalize-space(),'{questionnaireName}')]";
            var popupExactXPath = $"{FilterPopupXPath}//li[contains(@class,'e-list-item') and normalize-space()='{questionnaireName}']";
            var popupContainsXPath = $"{FilterPopupXPath}//li[contains(@class,'e-list-item') and contains(normalize-space(),'{questionnaireName}') ]";

            IWebElement option = null;
            try
            {
                option = BrowserHelper
                    .Wait("Timed out waiting for questionnaire option in filter list", TimeSpan.FromSeconds(5))
                    .Until(driver =>
                        driver.FindElements(By.XPath(popupExactXPath)).FirstOrDefault(candidate => candidate.Displayed) ??
                        driver.FindElements(By.XPath(popupContainsXPath)).FirstOrDefault(candidate => candidate.Displayed) ??
                        driver.FindElements(By.XPath(FilterPopupListItemXPath)).FirstOrDefault(candidate => candidate.Displayed) ??
                        driver.FindElements(By.XPath(exactMatchXPath)).FirstOrDefault(candidate => candidate.Displayed) ??
                        driver.FindElements(By.XPath(containsXPath)).FirstOrDefault(candidate => candidate.Displayed) ??
                        driver.FindElements(By.XPath(roleOptionXPath)).FirstOrDefault(candidate => candidate.Displayed));
            }
            catch (WebDriverTimeoutException)
            {
                // Some Syncfusion filter popups apply the typed text without a visible list.
            }

            if (option != null)
            {
                option.Click();
            }
        }

        private void ConfirmSurveyFilterIfPossible()
        {
            try
            {
                var button = BrowserHelper
                    .Wait("Timed out waiting for filter confirmation button", TimeSpan.FromSeconds(5))
                    .Until(driver =>
                    {
                        var popup = driver.FindElements(By.Id(InstrumentFilterPopupId))
                            .FirstOrDefault(candidate => candidate.Displayed);
                        if (popup != null)
                        {
                            return popup.FindElements(By.XPath(FilterPopupButtonXPath))
                                .FirstOrDefault(candidate => candidate.Displayed);
                        }

                        return driver.FindElements(By.XPath(FilterPopupButtonXPath))
                            .FirstOrDefault(candidate => candidate.Displayed);
                    });

                button?.Click();
            }
            catch (WebDriverTimeoutException)
            {
                // No confirmation button for this UI, filter applies on selection.
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

        protected override By PageIdentityBy => UseNewSelectors
            ? By.XPath("//div[@e-mappinguid='qa_instrumentid' and contains(@class, 'e-filtermenudiv')]")
            : By.XPath(FilterButton);
    }
}
