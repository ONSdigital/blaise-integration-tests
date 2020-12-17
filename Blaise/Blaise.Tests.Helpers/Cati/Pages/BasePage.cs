using System.Collections;
using System.Collections.Generic;
using Blaise.Tests.Helpers.Browser;
using OpenQA.Selenium;

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

        protected IList<IWebElement> GetTableContentById(string elementId)
        {
            return BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.TagName("td")))
                .FindElements(By.ClassName("table__cell"));
        }

        public void LoadPage()
        {
            BrowserHelper.BrowseTo(_pageUrl);
        }
    }
}
