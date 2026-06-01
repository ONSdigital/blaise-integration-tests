namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Linq;
    using System.Web;
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
        private const string FilterPopupXPath = "//div[contains(@class,'e-popup') and contains(@class,'e-popup-open')]";
        private static readonly string[] InstrumentFilterPopupIds = { "qa_instrumentid-flmdlg", "qa_instrument-flmdlg" };
        private const string FilterPopupButtonXPath = ".//button[contains(@class,'e-flmenu-okbtn') or normalize-space()='Filter' or normalize-space()='Apply']";
        private const string FilterPopupListItemXPath = "//div[contains(@class,'e-popup') and contains(@class,'e-popup-open')]//li[contains(@class,'e-list-item')]";
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

        private string QuestionnaireCellPath => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']//tr//td[@aria-colindex='1']"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[1]";

        private string CaseIdCellPath => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']//tr//td[@aria-colindex='2']"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";

        private string PlayButtonSelector => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']//tr[1]//a[starts-with(@id,'qa_startcase_')]"
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
                            playButton
                        );

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

            var query = HttpUtility.ParseQueryString(uri.Query);
            var targetUrl = query["url"];
            if (!string.IsNullOrWhiteSpace(targetUrl) && !targetUrl.EndsWith("/", StringComparison.Ordinal))
            {
                query["url"] = $"{targetUrl}/";
            }

            var builder = new UriBuilder(uri)
            {
                Query = query.ToString() ?? string.Empty,
            };

            return builder.Uri.ToString();
        }

        public void ApplyFilter()
        {
            if (UseNewSelectors)
            {
                var attempts = 0;
                while (true)
                {
                    try
                    {
                        ApplyFilterForNewUi();
                        return;
                    }
                    catch (StaleElementReferenceException ex)
                    {
                        attempts++;
                        Console.WriteLine($"Stale element while applying case info filter (attempt {attempts}): {ex.Message}");
                        if (attempts >= 3)
                        {
                            throw;
                        }
                    }
                }
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

        private IWebElement FindCaseInfoSearchInput(IWebDriver driver)
        {
            var activeElement = driver.SwitchTo().ActiveElement();
            if (activeElement != null &&
                activeElement.Displayed &&
                activeElement.TagName.Equals("input", StringComparison.OrdinalIgnoreCase) &&
                IsUsableFilterInput(activeElement))
            {
                return activeElement;
            }

            var popup = FindInstrumentFilterPopup(driver);
            if (popup != null)
            {
                var popupInput = popup
                    .FindElements(By.CssSelector("input[aria-label='multiselect'], input.e-multiselect, input[role='combobox'], input[type='text']"))
                    .FirstOrDefault(candidate => candidate.Displayed && IsUsableFilterInput(candidate));
                if (popupInput != null)
                {
                    return popupInput;
                }
            }

            var byAria = driver.FindElements(By.CssSelector("input[aria-label='multiselect']"))
                .FirstOrDefault(candidate => candidate.Displayed && IsUsableFilterInput(candidate));
            if (byAria != null)
            {
                return byAria;
            }

            return driver.FindElements(By.CssSelector("input[id^='multiselect-']"))
                .FirstOrDefault(candidate => candidate.Displayed && IsUsableFilterInput(candidate));
        }

        private bool SelectCaseInfoFilterOption(string questionnaireName)
        {
            var exactMatchXPath = $"//li[contains(@class,'e-list-item') and normalize-space()='{questionnaireName}']";
            var containsXPath = $"//li[contains(@class,'e-list-item') and contains(normalize-space(),'{questionnaireName}') ]";
            var roleOptionXPath = $"//li[@role='option' and contains(normalize-space(),'{questionnaireName}') ]";
            var popupExactXPath = $"{FilterPopupXPath}//li[contains(@class,'e-list-item') and normalize-space()='{questionnaireName}']";
            var popupContainsXPath = $"{FilterPopupXPath}//li[contains(@class,'e-list-item') and contains(normalize-space(),'{questionnaireName}') ]";
            for (var attempt = 0; attempt < 2; attempt++)
            {
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

                if (option == null)
                {
                    return false;
                }

                try
                {
                    option.Click();
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    if (attempt >= 1)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        private void ConfirmCaseInfoFilterIfPossible()
        {
            try
            {
                var button = BrowserHelper
                    .Wait("Timed out waiting for filter confirmation button", TimeSpan.FromSeconds(5))
                    .Until(driver =>
                    {
                        var popup = FindInstrumentFilterPopup(driver);
                        if (popup == null)
                        {
                            return driver.FindElements(By.XPath(FilterPopupButtonXPath))
                                .FirstOrDefault(candidate => candidate.Displayed);
                        }

                        return popup.FindElements(By.XPath(FilterPopupButtonXPath))
                            .FirstOrDefault(candidate => candidate.Displayed);
                    });

                if (button == null)
                {
                    return;
                }

                try
                {
                    button.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    BrowserHelper.ScrollIntoView(button);
                    BrowserHelper.ClickByXPathWithJavaScriptWithRetry(FilterPopupButtonXPath);
                }
                catch (StaleElementReferenceException)
                {
                    BrowserHelper.ClickByXPathWithJavaScriptWithRetry(FilterPopupButtonXPath);
                }
            }
            catch (WebDriverTimeoutException)
            {
                // No confirmation button for this UI, filter applies on selection.
            }
        }

        private void ApplyFilterForNewUi()
        {
            BrowserHelper.WaitUntilGridHasLoadedData();
            ResetCaseInfoGridHorizontalScroll();
            ClickInstrumentFilterIcon();

            BrowserHelper
                .Wait("Timed out waiting for filter popup to open")
                .Until(driver => FindInstrumentFilterPopup(driver) != null);

            var alreadySelected = false;
            try
            {
                alreadySelected = BrowserHelper
                    .Wait("Timed out waiting for existing case info filter selection", TimeSpan.FromSeconds(1))
                    .Until(driver => CaseInfoFilterAlreadySelected(driver, BlaiseConfigurationHelper.QuestionnaireName));
            }
            catch (WebDriverTimeoutException)
            {
                alreadySelected = false;
            }

            if (alreadySelected)
            {
                TabOffCaseInfoFilterInput();
                WaitForCaseInfoDropdownToClose();
                ConfirmCaseInfoFilterIfPossible();
                BrowserHelper.WaitUntilGridHasLoadedData();
                return;
            }

            var searchInput = BrowserHelper
                .Wait("Timed out waiting for case info filter search input")
                .Until(FindCaseInfoSearchInput);
            searchInput.Clear();
            searchInput.SendKeys(BlaiseConfigurationHelper.QuestionnaireName);

            var optionSelected = SelectCaseInfoFilterOption(BlaiseConfigurationHelper.QuestionnaireName);
            if (!optionSelected)
            {
                searchInput.SendKeys(Keys.Escape);
            }

            TabOffCaseInfoFilterInput(searchInput);
            WaitForCaseInfoDropdownToClose();
            ConfirmCaseInfoFilterIfPossible();
            BrowserHelper.WaitUntilGridHasLoadedData();
        }

        private void WaitForCaseInfoDropdownToClose()
        {
            try
            {
                BrowserHelper
                    .Wait("Timed out waiting for case info dropdown to close", TimeSpan.FromSeconds(3))
                    .Until(driver =>
                    {
                        var dropdownVisible = driver
                            .FindElements(By.CssSelector("div.e-content.e-dropdownbase"))
                            .Any(candidate => candidate.Displayed);
                        if (dropdownVisible)
                        {
                            return false;
                        }

                        var input = FindCaseInfoSearchInput(driver);
                        if (input == null)
                        {
                            return true;
                        }

                        var expanded = input.GetAttribute("aria-expanded");
                        return string.IsNullOrWhiteSpace(expanded) ||
                               expanded.Equals("false", StringComparison.OrdinalIgnoreCase);
                    });
            }
            catch (WebDriverTimeoutException)
            {
                // Continue and let the JS click fallback handle the filter action.
            }
            catch (StaleElementReferenceException)
            {
                // Treat a stale input as closed.
            }
        }

        private bool CaseInfoFilterAlreadySelected(IWebDriver driver, string questionnaireName)
        {
            try
            {
                var popup = FindInstrumentFilterPopup(driver);
                if (popup == null)
                {
                    return false;
                }

                var chips = popup.FindElements(By.CssSelector(".e-chips-collection, .e-delim-values"));
                return chips.Any(chip =>
                    chip.Displayed &&
                    !string.IsNullOrWhiteSpace(chip.Text) &&
                    chip.Text.IndexOf(questionnaireName, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        private bool IsUsableFilterInput(IWebElement input)
        {
            if (input == null || !input.Displayed || !input.Enabled)
            {
                return false;
            }

            var readOnly = input.GetAttribute("readonly");
            if (!string.IsNullOrWhiteSpace(readOnly))
            {
                return false;
            }

            var ariaLabel = input.GetAttribute("aria-label") ?? string.Empty;
            var id = input.GetAttribute("id") ?? string.Empty;
            var name = input.GetAttribute("name") ?? string.Empty;

            if (ariaLabel.Equals("multiselect", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return id.StartsWith("multiselect-", StringComparison.OrdinalIgnoreCase) ||
                   name.StartsWith("multiselect-", StringComparison.OrdinalIgnoreCase);
        }

        private IWebElement FindInstrumentFilterPopup(IWebDriver driver)
        {
            foreach (var popupId in InstrumentFilterPopupIds)
            {
                var popup = driver.FindElements(By.Id(popupId))
                    .FirstOrDefault(candidate => candidate.Displayed);
                if (popup != null)
                {
                    return popup;
                }
            }

            return driver.FindElements(By.XPath(FilterPopupXPath))
                .FirstOrDefault(candidate => candidate.Displayed);
        }

        private void TabOffCaseInfoFilterInput(IWebElement input = null)
        {
            try
            {
                if (input != null)
                {
                    input.SendKeys(Keys.Tab);
                    return;
                }

                BrowserHelper.ExecuteJavaScript("if (document.activeElement) { document.activeElement.blur(); }");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to tab off case info filter input: {ex.Message}");
            }
        }
    }
}
