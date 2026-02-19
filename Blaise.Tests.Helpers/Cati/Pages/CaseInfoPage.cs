namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Threading;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class CaseInfoPage : BasePage
    {
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

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
            ? "//*[@id='CaseInfo_content_table']/tbody/tr[1]/td[19]/div/div/div"
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

                    Thread.Sleep(250);
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
                ClickButtonByXPath("//div[contains(@class, 'e-filtermenudiv')]");
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
            return UseNewSelectors
                ? BodyDoesNotContainText("No records to display")
                : BodyContainsText("Showing");
        }

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
    }
}
