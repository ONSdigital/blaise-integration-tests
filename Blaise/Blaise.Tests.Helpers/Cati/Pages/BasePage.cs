using System.Collections.Generic;
using Blaise.Tests.Helpers.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Blaise.Tests.Helpers.Cati.Pages
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

        protected string GetElementTextByPath(string elementPath)
        {
            return BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(elementPath))).Text;
        }

        protected void PopulateTextBoxById(string elementId, string value)
        {
            BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(elementId)))
                .SendKeys(value);
        }

        protected void PopulateTextBoxByName(string elementName, string value)
        {
            BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name(elementName)))
                .SendKeys(value);
        }

        protected List<string> GetFirstColumnOfTableFromXPath(string tablePath, string tableId)
        {
            BrowserHelper.Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(tableId)));
            var numberOfRows = NumberOfRowsInATable(tablePath);
            var results = ReadFirstColumnInATable(tablePath, numberOfRows);

            return results;
        }

        protected void SelectDropDownListItem(string dropDownPath, string itemToSelect)
        {
            var dropdownList = BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(dropDownPath)))
                .FindElement(By.XPath(dropDownPath));
            var selectElement = new SelectElement(dropdownList);
            selectElement.SelectByText(itemToSelect);
        }

        protected void SuccessToastByClass(string toastSuccessClass)
        {
            BrowserHelper.Wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName(toastSuccessClass)));
        }

        public void LoadPage()
        {
            BrowserHelper.BrowseTo(_pageUrl);
        }

        public void ButtonIsAvailableByPath(string submitButtonPath)
        {
            BrowserHelper.Wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(submitButtonPath)));
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
