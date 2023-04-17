using Blaise.Tests.Helpers.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
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

        protected void ClickButtonById(string buttonElementId)
        {
            BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id(buttonElementId))).Click();
        }

        protected void ClickButtonByXPath(string buttonElementPath)
        {
            BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(buttonElementPath))).Click();
        }

        protected string GetElementTextById(string elementId)
        {
            var element = BrowserHelper.FindElement(By.Id(elementId));
            return element != null ? element.Text : "";
        }

        protected string GetElementTextByPath(string elementPath)
        {
            return BrowserHelper.Wait
                 .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(elementPath))).Text;
        }

        protected void PopulateInputById(string elementId, string value)
        {
            BrowserHelper.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(elementId)))
                .Clear();

            BrowserHelper.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(elementId)))
                .SendKeys(value);
        }

        protected void PopulateInputByName(string elementName, string value)
        {
            BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name(elementName)))
                .SendKeys(value);
        }

        protected bool CheckIfCheckboxIsCheckedByXPath(string elementName)
        {
            return BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(elementName)))
                .Selected;
        }

        protected List<string> GetFirstColumnOfTableFromXPath(string tablePath, string tableId)
        {
            BrowserHelper.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(tableId)));
            var numberOfRows = NumberOfRowsInATable(tablePath);
            var results = ReadFirstColumnInATable(tablePath, numberOfRows);

            return results;
        }

        protected void WaitForPageToChange(string url)
        {
            BrowserHelper.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlMatches(url));
        }

        public void LoadPage()
        {
            BrowserHelper.BrowseTo(_pageUrl);
        }

        public void LoadSpecificPage(string url)
        {
            BrowserHelper.BrowseTo(url);
        }

        public void ButtonIsAvailableById(string buttonId)
        {
            BrowserHelper.Wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id(buttonId)));
        }

        public void ButtonIsAvailableByPath(string submitButtonPath)
        {
            BrowserHelper.Wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(submitButtonPath)));
        }

        public void SelectDropDownValueById(string dropDownId, string value)
        {
            var selectList = new SelectElement(BrowserHelper.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(dropDownId))));
            selectList.SelectByText(value);
        }

        private static int NumberOfRowsInATable(string tablePath)
        {
            return BrowserHelper.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(tablePath)))
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
