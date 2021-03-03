using System;
using System.IO;
using System.Linq;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
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

        public static string TakeScreenShot(string screenShotPath, string screenShotName)
        {
            var screenShot = _browser.TakeScreenshot();
            var screenShotFile = Path.Combine(screenShotPath, $"{screenShotName}.png");
            screenShot.SaveAsFile(screenShotFile, ScreenshotImageFormat.Png);

            return screenShotFile;
        }

        public static void CloseBrowser()
        {
            Browser.Quit();
            _browser = null;
        }

        public static void BrowseTo(string pageUrl)
        {
            Browser.Navigate().GoToUrl(pageUrl);
        }

        public static void SwitchToLastOpenedWindow()
        {
            Browser.SwitchTo().Window(Browser.WindowHandles.Last());
        }

        private static ChromeDriver CreateChromeDriver()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AcceptInsecureCertificates = true;
            
            //chromeOptions.AddArguments("headless");
            //chromeOptions.AddArguments("start-maximized");

            return new ChromeDriver(BrowserConfigurationHelper.ChromeDriver, chromeOptions);
        }
    }
}