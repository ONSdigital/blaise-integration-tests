using System;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class BasePage
    {
        public string Title => BrowserHelper.CurrentTitle;
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
            return BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(elementId))).Text;
        }

        protected string GetElementTextByPath(string elementPath)
        {
            return BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(elementPath))).Text;
        }

        protected void PopulateTextboxById(string elementId, string value)
        {
            BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(elementId)))
                .SendKeys(value);
        }

        protected void PopulateTextboxByName(string elementName, string value)
        {
            BrowserHelper.Wait
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name(elementName)))
                .SendKeys(value);
        }

        public void LoadPage()
        {
            BrowserHelper.BrowseTo(_pageUrl);
        }
    }
}
