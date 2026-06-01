namespace Blaise.Tests.Helpers.Browser
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public static class ExpectedConditions
    {
        public static Func<IWebDriver, IWebElement> ElementIsVisible(By locator)
        {
            return driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed ? element : null;
            };
        }

        public static Func<IWebDriver, IWebElement> ElementExists(By locator)
        {
            return driver => driver.FindElement(locator);
        }

        public static Func<IWebDriver, IWebElement> ElementToBeClickable(By locator)
        {
            return driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed && element.Enabled ? element : null;
            };
        }

        public static Func<IWebDriver, bool> TextToBePresentInElement(IWebElement element, string text)
        {
            return driver =>
            {
                var elementText = element.Text;
                return elementText.Contains(text);
            };
        }

        public static Func<IWebDriver, bool> UrlContains(string fraction)
        {
            return driver => driver.Url.Contains(fraction);
        }
    }
}
