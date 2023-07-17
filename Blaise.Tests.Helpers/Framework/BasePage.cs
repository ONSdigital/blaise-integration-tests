using Blaise.Tests.Helpers.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace Blaise.Tests.Helpers.Framework
{
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

        protected void ClickButtonById(string buttonElementId)
        {
            var button = BrowserHelper
                .Wait($"Timed out in ClickButtonById(\"{buttonElementId}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id(buttonElementId)));

            BrowserHelper.ScrollIntoView(button);

            button.Click();
        }

        protected void ClickButtonByXPath(string buttonElementPath)
        {
            BrowserHelper
                .Wait($"Timed out in ClickButtonByXPath(\"{buttonElementPath}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(buttonElementPath))).Click();
        }

        protected string GetElementTextById(string elementId)
        {
            var element = BrowserHelper.FindElement(By.Id(elementId));
            return element != null ? element.Text : "";
        }

        protected string GetElementTextByPath(string elementPath)
        {
            return BrowserHelper
                 .Wait($"Timed out in GetElementTextByPath(\"{elementPath}\")")
                 .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(elementPath))).Text;
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
            BrowserHelper
                .Wait($"Timed out in PopulateInputById(\"{elementId}\", \"{value}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(elementId)))
                .Clear();

            BrowserHelper
                .Wait($"Timed out in PopulateInputById(\"{elementId}\", \"{value}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(elementId)))
                .SendKeys(value);
        }

        protected void PopulateInputByName(string elementName, string value)
        {
            /* BrowserHelper
                 .Wait($"Timed out in PopulateInputByName(\"{elementName}\", \"{value}\")")
                 .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name(elementName)))
             .SendKeys(value);*/

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
            //Are we currently on the required page
            if (!BrowserHelper.GetCurrentUrl().Equals(url, StringComparison.CurrentCultureIgnoreCase))
            {
                BrowserHelper.Wait($"Timed out in WaitForPageToChange(\"{url}\")")
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlMatches(url));
            }
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

        protected virtual Func<IWebDriver, bool> PageHasLoaded()
        {
            return driver => true;
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

        private static int NumberOfRowsInATable(string tablePath)
        {
            return BrowserHelper
                .Wait($"Timed out in NumberOfRowsInATable(\"{tablePath}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tablePath)))
                .FindElements(By.XPath(tablePath)).Count;
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
