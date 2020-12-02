using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Blaise.Tests.Helpers.Browser
{
    public static class BrowserHelper
    {

        private static IWebDriver _browser;

        private static IWebDriver Browser => _browser ?? (_browser = new ChromeDriver(BrowserConfigurationHelper.ChromeDriver));

        public static int TimeOutInSeconds => BrowserConfigurationHelper.TimeOutInSeconds;

        public static WebDriverWait Wait => new WebDriverWait(Browser, TimeSpan.FromSeconds(TimeOutInSeconds));

        public static string CurrentUrl => Browser.Url;
        public static string CurrentTitle => Browser.Title;

        public static void Dispose()
        {
            Browser.Quit();
        }

        public static void BrowseTo(string pageUrl)
        {
            Browser.Navigate().GoToUrl(pageUrl);
        }
    }
}
