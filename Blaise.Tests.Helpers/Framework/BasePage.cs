using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Cati;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;

public class BasePage
    {
        protected readonly string _pageUrl;

        public BasePage(string pageUrl)
        {
            _pageUrl = pageUrl;
        }

        public BasePage(string pageUrl, string pageUrlParameter)
        {
            _pageUrl = $"{pageUrl}?{pageUrlParameter}";
        }

        //public void LoadPage()
        //{
        //    BrowserHelper.BrowseTo(_pageUrl);
        //    BrowserHelper
        //        .Wait($"Timed out waiting for page {_pageUrl} to load")
        //        .Until(PageHasLoaded());
        //}

        //public void LoadSpecificPage(string url)
        //{
        //    BrowserHelper.BrowseTo(url);
        //}

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

        protected void ClickButtonById(string buttonElementId)
        {
            BrowserHelper
                .Wait($"Timed out in ClickButtonById(\"{buttonElementId}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id(buttonElementId))).Click();
        }

        protected void ClickButtonByXPath(string buttonElementPath)
        {
            var buttonElement = BrowserHelper.FindElement(By.XPath(buttonElementPath));
            BrowserHelper.ScrollIntoView(buttonElement); // Ensure the button is in view

            var button = BrowserHelper
                .Wait($"Timed out in ClickButtonByXPath(\"{buttonElementPath}\")")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(buttonElementPath)));

            button.Click();
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


        //protected void WaitForPageToChange(string url)
        //{
        //    var currentUrl = BrowserHelper.GetCurrentUrl();
        
        //    if (currentUrl.IndexOf(url, StringComparison.OrdinalIgnoreCase) < 0)
        //    {
        //        BrowserHelper.Wait($"WaitForPageToChange expected (\"{url}\") actual (\"{currentUrl}\") ")
        //                 .Until(d =>
        //                 {
        //                     var current = BrowserHelper.GetCurrentUrl().ToLowerInvariant();

        //                     return current.Contains("dst2304z")
        //                         && current.Contains("layoutset=cati-interviewer_large");
        //                 });
        //    }
        //}

        protected void WaitForPageToChange(string url)
        {
            if (!BrowserHelper.GetCurrentUrl().Contains(url))
            {
                string caseInsensitiveUrlPattern = $"(?i){url}";
                BrowserHelper.Wait($"WaitForPageToChange expected (\"{url}\") actual (\"{BrowserHelper.CurrentUrl}\") ")
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlMatches(caseInsensitiveUrlPattern));
            }
        }


    //protected static bool ElementIsDisplayed(By by)
    //{
    //    return BrowserHelper.ElementIsDisplayed(by);
    //}

    //protected static Func<IWebDriver, bool> BodyContainsText(string text)
    //{
    //    return driver =>
    //    {
    //        var body = driver.FindElement(By.TagName("body"));

    //        return SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(body, text)(driver);
    //    };
    //}

    //protected virtual Func<IWebDriver, bool> PageHasLoaded()
    //{
    //    return driver => true;
    //}

    public void LoadPage()
        {
            BrowserHelper.BrowseTo(_pageUrl);
            this.WaitForPageToChange(_pageUrl);
            BrowserHelper
                .Wait($"Timed out waiting for page {_pageUrl} to load")
                .Until(PageHasLoaded());
        }

        public void LoadSpecificPage(string url)
        {
            BrowserHelper.BrowseTo(url);
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
