namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Linq;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class DaybatchPage : BasePage
    {
        private const string DaybatchCreateButtonId = "btnCreateDaybatch";
        private const string QuestionnaireDropDownId = "InstrumentId";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";
        private const string InstrumentFilterIconXPath = "//div[contains(@class,'e-filtermenudiv') and (@e-mappinguid='qa_instrumentid' or @e-mappinguid='qa_instrument')]";
        private const string FilterPopupXPath = "//div[contains(@class,'e-popup') and contains(@class,'e-popup-open')]";
        private static readonly string[] InstrumentFilterPopupIds = { "qa_instrumentid-flmdlg", "qa_instrument-flmdlg" };
        private const string FilterPopupButtonXPath = ".//button[contains(@class,'e-flmenu-okbtn') or normalize-space()='Filter' or normalize-space()='Apply']";
        private const string FilterPopupListItemXPath = "//div[contains(@class,'e-popup') and contains(@class,'e-popup-open')]//li[contains(@class,'e-list-item')]";

        private bool UseNewSelectors
        {
            get
            {
                return CatiUiVersionHelper.IsNewUi;
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

        private string DayBatchTableSelector => UseNewSelectors
            ? "//*[@id='Daybatch_content_table']"
            : "//*[@id='MVCGridTable_DaybatchGrid']";

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
                        Console.WriteLine($"Stale element while applying daybatch filter (attempt {attempts}): {ex.Message}");
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

        private void ApplyFilterForNewUi()
        {
            BrowserHelper.WaitUntilGridHasLoadedData();
            ClickInstrumentFilterIcon();

            BrowserHelper
                .Wait("Timed out waiting for daybatch filter popup to open")
                .Until(driver => FindInstrumentFilterPopup(driver) != null);

            var alreadySelected = false;
            try
            {
                alreadySelected = BrowserHelper
                    .Wait("Timed out waiting for existing daybatch filter selection", TimeSpan.FromSeconds(1))
                    .Until(driver => DaybatchFilterAlreadySelected(driver, BlaiseConfigurationHelper.QuestionnaireName));
            }
            catch (WebDriverTimeoutException)
            {
                alreadySelected = false;
            }

            if (alreadySelected)
            {
                TabOffDaybatchFilterInput();
                WaitForDaybatchDropdownToClose();
                ConfirmDaybatchFilterIfPossible();
                BrowserHelper.WaitUntilGridHasLoadedData();
                return;
            }

            var searchInput = BrowserHelper
                .Wait("Timed out waiting for daybatch filter search input")
                .Until(FindDaybatchSearchInput);
            searchInput.Clear();
            searchInput.SendKeys(BlaiseConfigurationHelper.QuestionnaireName);

            var optionSelected = SelectDaybatchFilterOption(BlaiseConfigurationHelper.QuestionnaireName);
            if (!optionSelected)
            {
                searchInput.SendKeys(Keys.Escape);
            }

            TabOffDaybatchFilterInput(searchInput);
            WaitForDaybatchDropdownToClose();
            ConfirmDaybatchFilterIfPossible();
            BrowserHelper.WaitUntilGridHasLoadedData();
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

        private IWebElement FindDaybatchSearchInput(IWebDriver driver)
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

        private bool SelectDaybatchFilterOption(string questionnaireName)
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
                        .Wait("Timed out waiting for questionnaire option in daybatch filter list", TimeSpan.FromSeconds(5))
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

        private void ConfirmDaybatchFilterIfPossible()
        {
            try
            {
                var button = BrowserHelper
                    .Wait("Timed out waiting for daybatch filter confirmation button", TimeSpan.FromSeconds(5))
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

        private void WaitForDaybatchDropdownToClose()
        {
            try
            {
                BrowserHelper
                    .Wait("Timed out waiting for daybatch dropdown to close", TimeSpan.FromSeconds(3))
                    .Until(driver =>
                    {
                        var dropdownVisible = driver
                            .FindElements(By.CssSelector("div.e-content.e-dropdownbase"))
                            .Any(candidate => candidate.Displayed);
                        if (dropdownVisible)
                        {
                            return false;
                        }

                        var input = FindDaybatchSearchInput(driver);
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

        private bool DaybatchFilterAlreadySelected(IWebDriver driver, string questionnaireName)
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

        private void TabOffDaybatchFilterInput(IWebElement input = null)
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
                Console.WriteLine($"Unable to tab off daybatch filter input: {ex.Message}");
            }
        }

        public void WaitForDaybatchTable()
        {
            BrowserHelper.WaitForElementByXPath(DayBatchTableSelector);
            if (UseNewSelectors)
            {
                BrowserHelper.WaitUntilGridHasLoadedData();
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

                    BrowserHelper.WaitForUrlToMatch(
                        UseNewSelectors ? CatiConfigurationHelper.NewDashboardDaybatchUrl : CatiConfigurationHelper.DaybatchUrl,
                        10);

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
                        continue;
                    }

                    // Validate the current URL explicitly
                    if (currentUrl.Contains("Daybatch"))
                    {
                        Console.WriteLine("Successfully navigated to the Daybatch page.");

                        // Wait for the Daybatch table to load
                        if (BrowserHelper.ElementExistsByXPath(DayBatchTableSelector, TimeSpan.FromSeconds(30)))
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
            }

            throw new Exception("Failed to navigate to the Daybatch page after multiple attempts. Ensure the URL and page structure are correct.");
        }

        // Added a public property to expose the UseNewSelectors logic
        public bool IsUsingNewSelectors => UseNewSelectors;

        protected override By PageIdentityBy => By.XPath(DayBatchTableSelector);
    }
}
