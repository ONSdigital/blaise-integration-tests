namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class CaseInfoPage : BasePage
    {
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";
        private const string InstrumentFilterIconXPath = "//div[contains(@class,'e-filtermenudiv') and (@e-mappinguid='qa_instrumentid' or @e-mappinguid='qa_instrument')]";
        private const string CaseInfoSearchBoxId = "CaseInfo_SearchBox";
        private const string InstrumentFilterApplyButtonId = "qa_instrument_excelDlg";
        private const string InstrumentFilterApplyButtonIdAlternate = "qa_instrumentid_excelDlg";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        private bool UseNewSelectors
        {
            get
            {
                return CatiUiVersionHelper.IsNewUi;
            }
        }

        public void NavigateToVersionSpecificPage()
        {
            if (UseNewSelectors)
            {
                BrowserHelper.NavigateToPage(CatiConfigurationHelper.NewDashboardCaseInfoUrl);
            }
            else
            {
                BrowserHelper.NavigateToPage(CatiConfigurationHelper.CaseInfoUrl);
            }
        }

        private string QuestionnaireCellPath => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']//tr//td[@aria-colindex='1']"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[1]";

        private string CaseIdCellPath => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']//tr//td[@aria-colindex='2']"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";

        private string PlayButtonSelector => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']/tbody/tr[1]/td[19]/div/div/a"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a/span";

        public CaseInfoPage()
            : base(CatiConfigurationHelper.CaseInfoUrl)
        {
        }

        public void RefreshPageUntilCaseIsPlayable(string caseId)
        {
            var attempts = 0;
            do
            {
                NavigateToVersionSpecificPage();
                ApplyFilter();

                if (UseNewSelectors)
                {
                    BrowserHelper.WaitUntilGridHasLoadedData();
                }

                WaitUntilFirstCaseQuestionnaireIs(BlaiseConfigurationHelper.QuestionnaireName);
                WaitUntilFirstCaseIs(caseId);

                Console.WriteLine($"Attempt {attempts + 1}: Checking if play button is playable...");
                Console.WriteLine($"UseNewSelectors: {UseNewSelectors}");
                Console.WriteLine($"Play button visible: {ElementIsDisplayed(By.XPath(PlayButtonSelector))}");

                attempts++;
                if (attempts > 5)
                {
                    throw new Exception("Giving up after 5 attempts waiting for play button");
                }
            }
            while (!FirstCaseIsPlayable());
        }

        public void ClickPlayButton()
        {
            var numberOfWindows = BrowserHelper.GetNumberOfWindows();
            var attempts = 0;

            while (BrowserHelper.GetNumberOfWindows() == numberOfWindows)
            {
                try
                {
                    if (UseNewSelectors)
                    {
                        // Locate the table's scrollable container
                        var tableScrollableContainer = BrowserHelper.FindElement(By.XPath("//*[@id='CaseInfo_content_table']/parent::div"));

                        // Locate the Play button
                        var playButton = BrowserHelper.FindElement(By.XPath(PlayButtonSelector));

                        // Scroll the table horizontally to bring the Play button into view
                        BrowserHelper.ExecuteJavaScript(
                            "arguments[0].scrollLeft = arguments[1].offsetLeft;",
                            tableScrollableContainer,
                            playButton
                        );

                        // Click the Play button
                        BrowserHelper.ScrollIntoViewAndClick(By.XPath(PlayButtonSelector));
                    }
                    else
                    {
                        BrowserHelper.ClickByXPathWithJavaScriptWithRetry(PlayButtonSelector);
                    }
                    BrowserHelper.WaitForWindowCount(numberOfWindows + 1, 10);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while clicking Play button: {ex.Message}");
                }

                attempts++;
                if (attempts > 5)
                {
                    throw new Exception("Timed out waiting for new window to open.");
                }
            }
        }

        public void ApplyFilter()
        {
            if (UseNewSelectors)
            {
                BrowserHelper.WaitUntilGridHasLoadedData();
                ResetCaseInfoGridHorizontalScroll();
                ClickInstrumentFilterIcon();
                PopulateInputById(CaseInfoSearchBoxId, BlaiseConfigurationHelper.QuestionnaireName);
                ClickInstrumentFilterApplyButton();
                BrowserHelper.WaitUntilGridHasLoadedData();
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

        public bool FirstCaseIsPlayable()
        {
            try
            {
                var isDisplayed = UseNewSelectors
                    ? ElementIsDisplayed(By.XPath(PlayButtonSelector))
                    : ElementIsDisplayed(By.XPath(PlayButtonSelector));

                if (isDisplayed)
                {
                    var playButton = BrowserHelper.FindElement(By.XPath(PlayButtonSelector));
                    return playButton.Enabled && playButton.Displayed;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking play button state: {ex.Message}");
                return false;
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
            ? By.XPath("//*[@id='CaseInfo_content_table']")
            : By.XPath("//*[@id='MVCGridTable_CaseInfoGrid']");

        private void WaitUntilFirstCaseQuestionnaireIs(string questionnaire)
        {
            var path = UseNewSelectors
                ? "//*[@id='CaseInfo_content_table']//tr[1]/td[@aria-colindex='1']"
                : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[1]";

            WaitUntilElementByXPathContainsText(path, questionnaire);
        }

        private void WaitUntilFirstCaseIs(string caseId)
        {
            var path = UseNewSelectors
                ? "//*[@id='CaseInfo_content_table']//tr[1]/td[@aria-colindex='2']"
                : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";

            WaitUntilElementByXPathContainsText(path, caseId);
        }

        private void ResetCaseInfoGridHorizontalScroll()
        {
            try
            {
                var tableScrollableContainer = BrowserHelper.FindElement(By.XPath("//*[@id='CaseInfo_content_table']/parent::div"));
                BrowserHelper.ExecuteJavaScript("arguments[0].scrollLeft = 0;", tableScrollableContainer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to reset CaseInfo grid horizontal scroll: {ex.Message}");
            }
        }

        private void ClickInstrumentFilterIcon()
        {
            try
            {
                BrowserHelper.ScrollIntoViewAndClick(By.XPath(InstrumentFilterIconXPath));
            }
            catch (WebDriverTimeoutException)
            {
                BrowserHelper.ClickByXPathWithJavaScriptWithRetry(InstrumentFilterIconXPath);
            }
        }

        private void ClickInstrumentFilterApplyButton()
        {
            if (BrowserHelper.ElementExistsById(InstrumentFilterApplyButtonId, TimeSpan.FromSeconds(2)))
            {
                BrowserHelper.ClickByIdWithRetry(InstrumentFilterApplyButtonId);
                return;
            }

            if (BrowserHelper.ElementExistsById(InstrumentFilterApplyButtonIdAlternate, TimeSpan.FromSeconds(2)))
            {
                BrowserHelper.ClickByIdWithRetry(InstrumentFilterApplyButtonIdAlternate);
                return;
            }

            ClickButtonById(InstrumentFilterApplyButtonId);
        }
    }
}
