namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class CaseInfoPage : BasePage
    {
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        public CaseInfoPage()
            : base(CatiConfigurationHelper.CaseInfoUrl)
        {
        }

        protected override By PageIdentityBy => UseNewSelectors
            ? By.XPath("//*[@id='CaseInfo_content_table']")
            : By.XPath("//*[@id='MVCGridTable_CaseInfoGrid']");

        private bool UseNewSelectors
        {
            get
            {
                return CatiUiVersionHelper.IsNewUi;
            }
        }

        private string QuestionnaireCellPath => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']//tr//td[@aria-colindex='1']"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[1]";

        private string CaseIdCellPath => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']//tr//td[@aria-colindex='2']"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";

        private string PlayButtonSelector => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']//tr[1]//a[starts-with(@id,'qa_startcase_')]"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a/span";

        public void NavigateToVersionSpecificPage()
        {
            var newUrl = CatiConfigurationHelper.NewDashboardCaseInfoUrl;
            var oldUrl = CatiConfigurationHelper.CaseInfoUrl;
            var preferNew = UseNewSelectors;

            BrowserHelper.NavigateToPage(preferNew ? newUrl : oldUrl);

            if (preferNew)
            {
                if (!IsCaseInfoGridLoaded(true) && IsCaseInfoGridLoaded(false))
                {
                    Console.WriteLine("New Case Info grid not detected. Falling back to legacy URL.");
                    BrowserHelper.NavigateToPage(oldUrl);
                }
            }
            else
            {
                if (!IsCaseInfoGridLoaded(false) && IsCaseInfoGridLoaded(true))
                {
                    Console.WriteLine("Legacy Case Info grid not detected. Falling back to new dashboard URL.");
                    BrowserHelper.NavigateToPage(newUrl);
                }
            }
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
                        var playButton = BrowserHelper.FindElements(By.XPath(PlayButtonSelector))
                            .FirstOrDefault();
                        if (playButton == null)
                        {
                            throw new Exception("Play button not found in the first row.");
                        }

                        var startSurveyUrl = GetStartSurveyUrl(playButton);
                        if (!string.IsNullOrWhiteSpace(startSurveyUrl))
                        {
                            Console.WriteLine($"Opening start survey URL: {startSurveyUrl}");
                            BrowserHelper.ExecuteJavaScript("window.open(arguments[0], '_blank');", startSurveyUrl);
                            BrowserHelper.WaitForWindowCount(numberOfWindows + 1, 10);
                            return;
                        }

                        // Scroll the table horizontally to bring the Play button into view
                        BrowserHelper.ExecuteJavaScript(
                            "arguments[0].scrollLeft = arguments[1].offsetLeft;",
                            tableScrollableContainer,
                            playButton);

                        // Click the Play button
                        try
                        {
                            playButton.Click();
                        }
                        catch (Exception)
                        {
                            BrowserHelper.ExecuteJavaScript("arguments[0].click();", playButton);
                        }
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
                ResetCaseInfoGridHorizontalScroll();
                SyncfusionGridFilterHelper.ApplyNewUiFilterWithRetry(BlaiseConfigurationHelper.QuestionnaireName);
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
                if (UseNewSelectors)
                {
                    if (!BrowserHelper.ElementExistsByXPath(PlayButtonSelector, TimeSpan.FromSeconds(2)))
                    {
                        return false;
                    }

                    var playButton = BrowserHelper.FindElements(By.XPath(PlayButtonSelector))
                        .FirstOrDefault();
                    return playButton != null && playButton.Enabled;
                }

                var isDisplayed = ElementIsDisplayed(By.XPath(PlayButtonSelector));
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

        private static bool IsStartSurveyUrl(string candidate)
        {
            return candidate.IndexOf("/CaseInfo/StartSurvey", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string NormalizeStartSurveyUrl(string startSurveyUrl)
        {
            if (string.IsNullOrWhiteSpace(startSurveyUrl))
            {
                return startSurveyUrl;
            }

            if (!Uri.TryCreate(startSurveyUrl, UriKind.Absolute, out var uri))
            {
                if (!Uri.TryCreate(new Uri(CatiConfigurationHelper.CatiBaseUrl), startSurveyUrl, out uri))
                {
                    return startSurveyUrl;
                }
            }

            var query = uri.Query;
            if (string.IsNullOrWhiteSpace(query))
            {
                return uri.ToString();
            }

            var updatedPairs = new List<string>();
            var updated = false;
            foreach (var rawPair in query.TrimStart('?').Split('&'))
            {
                if (string.IsNullOrWhiteSpace(rawPair))
                {
                    continue;
                }

                var parts = rawPair.Split(new[] { '=' }, 2);
                var key = WebUtility.UrlDecode(parts[0] ?? string.Empty);
                var value = parts.Length > 1 ? WebUtility.UrlDecode(parts[1] ?? string.Empty) : string.Empty;

                if (string.Equals(key, "url", StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(value) &&
                    !value.EndsWith("/", StringComparison.Ordinal))
                {
                    value = $"{value}/";
                    updated = true;
                }

                var encodedKey = WebUtility.UrlEncode(key);
                var encodedValue = WebUtility.UrlEncode(value);
                updatedPairs.Add($"{encodedKey}={encodedValue}");
            }

            if (!updated)
            {
                return uri.ToString();
            }

            var builder = new UriBuilder(uri)
            {
                Query = string.Join("&", updatedPairs),
            };

            return builder.Uri.ToString();
        }

        private string GetStartSurveyUrl(IWebElement playButton)
        {
            var attributeCandidates = new[] { "href", "data-url", "data-start-url", "data-href" };
            foreach (var attribute in attributeCandidates)
            {
                var value = playButton.GetAttribute(attribute);
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                if (!IsStartSurveyUrl(value))
                {
                    continue;
                }

                return NormalizeStartSurveyUrl(value);
            }

            return null;
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

        private bool IsCaseInfoGridLoaded(bool isNewUi)
        {
            var selector = isNewUi
                ? "//*[@id='CaseInfo_content_table']"
                : "//*[@id='MVCGridTable_CaseInfoGrid']";
            return BrowserHelper.ElementExistsByXPath(selector, TimeSpan.FromSeconds(5));
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
    }
}
