namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Linq;
    using Blaise.Tests.Helpers.Browser;
    using OpenQA.Selenium;

    public static class SyncfusionGridFilterHelper
    {
        private const string InstrumentFilterIconXPath = "//div[contains(@class,'e-filtermenudiv') and (@e-mappinguid='qa_instrumentid' or @e-mappinguid='qa_instrument')]";
        private const string FilterPopupXPath = "//div[contains(@class,'e-popup') and contains(@class,'e-popup-open')]";
        private const string FilterPopupButtonXPath = ".//button[contains(@class,'e-flmenu-okbtn') or normalize-space()='Filter' or normalize-space()='Apply']";
        private const string FilterPopupListItemXPath = "//div[contains(@class,'e-popup') and contains(@class,'e-popup-open')]//li[contains(@class,'e-list-item')]";
        private static readonly string[] InstrumentFilterPopupIds = { "qa_instrumentid-flmdlg", "qa_instrument-flmdlg" };

        public static void ApplyNewUiFilter(string questionnaireName)
        {
            BrowserHelper.WaitUntilGridHasLoadedData();
            ClickInstrumentFilterIcon();

            BrowserHelper
                .Wait("Timed out waiting for filter popup to open")
                .Until(driver => FindInstrumentFilterPopup(driver) != null);

            if (FilterAlreadySelected(questionnaireName))
            {
                TabOffFilterInput();
                WaitForDropdownToClose();
                ConfirmFilterIfPossible();
                BrowserHelper.WaitUntilGridHasLoadedData();
                return;
            }

            var searchInput = BrowserHelper
                .Wait("Timed out waiting for filter search input")
                .Until(FindSearchInput);
            searchInput.Clear();
            searchInput.SendKeys(questionnaireName);

            var optionSelected = SelectFilterOption(questionnaireName);
            if (!optionSelected)
            {
                searchInput.SendKeys(Keys.Escape);
            }

            TabOffFilterInput(searchInput);
            WaitForDropdownToClose();
            ConfirmFilterIfPossible();
            BrowserHelper.WaitUntilGridHasLoadedData();
        }

        public static void ApplyNewUiFilterWithRetry(string questionnaireName, int maxAttempts = 3)
        {
            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                try
                {
                    ApplyNewUiFilter(questionnaireName);
                    return;
                }
                catch (StaleElementReferenceException ex)
                {
                    Console.WriteLine($"Stale element while applying filter (attempt {attempt + 1}): {ex.Message}");
                    if (attempt >= maxAttempts - 1)
                    {
                        throw;
                    }
                }
            }
        }

        public static IWebElement FindInstrumentFilterPopup(IWebDriver driver)
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

        public static bool IsUsableFilterInput(IWebElement input)
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

        private static void ClickInstrumentFilterIcon()
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

        private static bool FilterAlreadySelected(string questionnaireName)
        {
            try
            {
                return BrowserHelper
                    .Wait("Checking for existing filter selection", TimeSpan.FromSeconds(1))
                    .Until(driver =>
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
                    });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        private static IWebElement FindSearchInput(IWebDriver driver)
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

        private static bool SelectFilterOption(string questionnaireName)
        {
            var exactMatchXPath = $"//li[contains(@class,'e-list-item') and normalize-space()='{questionnaireName}']";
            var containsXPath = $"//li[contains(@class,'e-list-item') and contains(normalize-space(),'{questionnaireName}') ]";
            var roleOptionXPath = $"//li[@role='option' and contains(normalize-space(),'{questionnaireName}')]";
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

        private static void ConfirmFilterIfPossible()
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

        private static void TabOffFilterInput(IWebElement input = null)
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
                Console.WriteLine($"Unable to tab off filter input: {ex.Message}");
            }
        }

        private static void WaitForDropdownToClose()
        {
            try
            {
                BrowserHelper
                    .Wait("Timed out waiting for dropdown to close", TimeSpan.FromSeconds(3))
                    .Until(driver =>
                    {
                        var dropdownVisible = driver
                            .FindElements(By.CssSelector("div.e-content.e-dropdownbase"))
                            .Any(candidate => candidate.Displayed);
                        if (dropdownVisible)
                        {
                            return false;
                        }

                        var input = FindSearchInput(driver);
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
    }
}
