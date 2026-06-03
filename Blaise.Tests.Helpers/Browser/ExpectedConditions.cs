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
                try
                {
                    var element = driver.FindElement(locator);
                    return element.Displayed ? element : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
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
                try
                {
                    var element = ElementIsVisible(locator)(driver);
                    if (element != null && element.Enabled)
                    {
                        return element;
                    }

                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        public static Func<IWebDriver, bool> TextToBePresentInElement(IWebElement element, string text)
        {
            return driver =>
            {
                try
                {
                    return element.Text.Contains(text);
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            };
        }

        public static Func<IWebDriver, bool> UrlContains(string fraction)
        {
            return driver => driver.Url.Contains(fraction);
        }
    }
}
