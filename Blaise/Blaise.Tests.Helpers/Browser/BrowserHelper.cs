using System;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Blaise.Tests.Helpers.Browser
{
    public static class BrowserHelper
    {

        private static IWebDriver _browser;

        private static IWebDriver Browser => _browser ?? (_browser = CreateChromeDriver());

        public static int TimeOutInSeconds => BrowserConfigurationHelper.TimeOutInSeconds;

        public static WebDriverWait Wait => new WebDriverWait(Browser, TimeSpan.FromSeconds(TimeOutInSeconds));

        public static string CurrentUrl => Browser.Url;

        public static void CloseBrowser()
        {
            Browser.Quit();
            _browser = null;
        }

        public static void BrowseTo(string pageUrl)
        {
            Browser.Navigate().GoToUrl(pageUrl);
        }

        private static ChromeDriver CreateChromeDriver()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("headless");
            return new ChromeDriver(BrowserConfigurationHelper.ChromeDriver, chromeOptions);
        }
    }
}