using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class BasePage
    {
        public string Title => Browser.Title;
        protected IWebDriver Browser;
        private readonly string _pageUrl;
        public BasePage(IWebDriver browser, string pageUrl)
        {
            Browser = browser;
            _pageUrl = pageUrl;
        }

        protected void ClickButtonById(string buttonElementId)
        {
            new WebDriverWait(Browser, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id(buttonElementId))).Click();
        }

        protected void ClickButtonByXPath(string buttonElementPath)
        {
            new WebDriverWait(Browser, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(buttonElementPath))).Click();
        }

        protected string GetElementTextById(string elementId)
        {
            return new WebDriverWait(Browser, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(elementId))).Text;
        }

        protected string GetElementTextByPath(string elementPath)
        {
            return new WebDriverWait(Browser, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(elementPath))).Text;
        }

        protected void PopulateTextboxById(string elementId, string value)
        {
            new WebDriverWait(Browser, TimeSpan.FromSeconds(5))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(elementId)))
                .SendKeys(value);
        }

        public void LoadPage()
        {
            Browser.Navigate().GoToUrl(_pageUrl);
        }
    }
}
