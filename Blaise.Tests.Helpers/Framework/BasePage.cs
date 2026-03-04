namespace Blaise.Tests.Helpers.Framework
{
    using System;
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Browser;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class BasePage
    {
        private readonly string _pageUrl;

        public BasePage(string pageUrl)
        {
            _pageUrl = pageUrl;
        }

        public BasePage(string pageUrl, string pageUrlParameter)
        {
            _pageUrl = $"{pageUrl}?{pageUrlParameter}";
        }

        public void LoadPage()
        {
            BrowserHelper.BrowseTo(_pageUrl);
            BrowserHelper
                .Wait($"Timed out waiting for page {_pageUrl} to load")
                .Until(PageHasLoaded());
        }

        public void LoadSpecificPage(string url)
        {
            BrowserHelper.BrowseTo(url);
        }

        public void ButtonIsAvailableById(string buttonId)
        {
            BrowserHelper
                .Wait($"Timed out in ButtonIsAvailableById(\"{buttonId}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id(buttonId)));
        }

        public void ButtonIsAvailableByPath(string submitButtonPath)
        {
            BrowserHelper
                .Wait($"Timed out in ButtonIsAvailableByPath(\"{submitButtonPath}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(submitButtonPath)));
        }

        public void SelectDropDownValueById(string dropDownId, string value)
        {
            var selectList = new SelectElement(
                BrowserHelper
                .Wait($"Timed out in SelectDropDownValueById(\"{dropDownId}\", \"{value}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(dropDownId))));
            selectList.SelectByText(value);
        }

        protected static bool ElementIsDisplayed(By by)
        {
            return BrowserHelper.ElementIsDisplayed(by);
        }

        protected static Func<IWebDriver, bool> BodyContainsText(string text)
        {
            return driver =>
            {
                var body = driver.FindElement(By.TagName("body"));

                return SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(body, text)(driver);
            };
        }

        protected static Func<IWebDriver, bool> BodyDoesNotContainText(string text)
        {
            return driver =>
            {
                try
                {
                    var body = driver.FindElement(By.TagName("body"));
                    return !body.Text.Contains(text);
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            };
        }

        protected void ClickButtonById(string buttonElementId)
        {
            BrowserHelper
                .Wait($"Timed out in ClickButtonById(\"{buttonElementId}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id(buttonElementId)))
                .Click();
        }

        protected void ClickButtonByXPath(string buttonElementPath)
        {
            var attempts = 0;
            while (attempts < 5)
            {
                try
                {
                    BrowserHelper
                        .Wait($"Timed out in ClickButtonByXPath(\"{buttonElementPath}\")")
                        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(buttonElementPath)))
                        .Click();
                    return;
                }
                catch (StaleElementReferenceException)
                {
                    attempts++;
                    if (attempts >= 5) throw;
                    System.Threading.Thread.Sleep(250);
                }
            }
        }

        protected void ClickButtonByXPathWithJavaScript(string buttonElementPath)
        {
            BrowserHelper.ClickWithJavaScript(By.XPath(buttonElementPath));
        }

        protected string GetElementTextById(string elementId)
        {
            return BrowserHelper.FindElement(By.Id(elementId)).Text;
        }

        protected string GetElementTextByPath(string elementPath)
        {
            var attempts = 0;
            while (attempts < 5)
            {
                try
                {
                    return BrowserHelper
                        .Wait($"Timed out in GetElementTextByPath(\"{elementPath}\")")
                        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(elementPath)))
                        .Text;
                }
                catch (StaleElementReferenceException)
                {
                    attempts++;
                    if (attempts >= 5) throw;
                    System.Threading.Thread.Sleep(250);
                }
            }
            return string.Empty;
        }

        protected void WaitUntilElementByXPathContainsText(string elementPath, string text)
        {
            try
            {
                var wait = BrowserHelper.Wait(
                    $"Timed out in WaitUntilXPathContainsText(\"{elementPath}\", \"{text}\")");
                wait.Until(driver =>
                {
                    var element = driver.FindElement(By.XPath(elementPath));
                    return SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(element, text)(driver);
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                var element = BrowserHelper.FindElement(By.XPath(elementPath));
                throw new WebDriverTimeoutException($"{ex.Message} (element currently contains \"{element.Text}\")", ex);
            }
        }

        protected void PopulateInputById(string elementId, string value)
        {
            var element = BrowserHelper
                .Wait($"Timed out in PopulateInputById(\"{elementId}\", \"{value}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(elementId)));
            element.Clear();
            element.SendKeys(value);
        }

        protected void PopulateInputByName(string elementName, string value)
        {
            BrowserHelper.PopulateInputByName(elementName, value);
        }

        protected List<string> GetFirstColumnOfTableFromXPath(string tablePath, string tableId)
        {
            BrowserHelper
                .Wait($"Timed out in GetFirstColumnOfTableFromXPath(\"{tablePath}\", \"{tableId}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(tableId)));
            var numberOfRows = NumberOfRowsInATable(tablePath);
            var results = ReadFirstColumnInATable(tablePath, numberOfRows);

            return results;
        }

        protected void WaitForPageToChange(string url)
        {
            if (!BrowserHelper.GetCurrentUrl().Contains(url))
            {
                BrowserHelper.Wait($"Timed out in WaitForPageToChange(\"{url}\")")
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains(url));
            }
        }

        protected bool ElementExistsById(string elementId, TimeSpan? timeout = null)
        {
            try
            {
                BrowserHelper
                    .Wait($"Timed out in ElementExistsById(\"{elementId}\")", timeout)
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(elementId)));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        protected virtual Func<IWebDriver, bool> PageHasLoaded()
        {
            return driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete");
        }

        private static int NumberOfRowsInATable(string tablePath)
        {
            return BrowserHelper
                .Wait($"Timed out in NumberOfRowsInATable(\"{tablePath}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tablePath)))
                .FindElements(By.XPath(tablePath))
                .Count;
        }

        private List<string> ReadFirstColumnInATable(string tablePath, int numberOfRows)
        {
            var questionnaires = new List<string>();
            for (var i = 1; i < numberOfRows + 1; i++)
            {
                var colPath = $"{tablePath}[{i}]/td[1]";
                questionnaires.Add(GetElementTextByPath(colPath));
            }

            return questionnaires;
        }
    }
}
