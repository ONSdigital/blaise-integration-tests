using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;

namespace Blaise.Tests.Helpers.Browser
{
    public static class BrowserHelper
    {

        private static IWebDriver _browser;

        private static IWebDriver Browser => _browser ?? (_browser = CreateChromeDriver());

        public static int TimeOutInSeconds => BrowserConfigurationHelper.TimeOutInSeconds;

        public static WebDriverWait Wait => new WebDriverWait(Browser, TimeSpan.FromSeconds(TimeOutInSeconds));

        public static string CurrentUrl => Browser.Url;

        public static bool ElementExistsByXpath(string xPath)
        {
            return Browser.FindElements(By.XPath(xPath)).Count > 0;
        }

        public static void ScrollHorizontalByOffset(int offset)
        {
            var actions = new Actions(Browser);

            actions.MoveByOffset(offset, 0);

            actions.Perform();
        }

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

        public static string CurrentWindowHTML()
        {
            return Browser.PageSource;
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
            var chromeOptions = new ChromeOptions
            {
                AcceptInsecureCertificates = true
            };

            //chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments("start-maximized");

            return new ChromeDriver(BrowserConfigurationHelper.ChromeDriver, chromeOptions);
        }

        public static void WaitForTextInHTML(string text)
        {
            Wait.Until(driver => CurrentWindowHTML().Contains(text));
        }

        public static void WaitForElementByXpath(string xPath, int timeoutInSeconds = 20)
        {
            FindElement(By.XPath(xPath), timeoutInSeconds);
        }

        public static IWebElement FindElement(By by, int timeoutInSeconds = 20, int pollingIntervalInMilliseconds = 500)
        {
            var timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            var pollingInterval = TimeSpan.FromMilliseconds(pollingIntervalInMilliseconds);

            try
            {
                var wait = new DefaultWait<IWebDriver>(_browser)
                {
                    Timeout = timeout,
                    PollingInterval = pollingInterval,
                    Message = $"Timed out after {timeoutInSeconds} seconds while waiting for element with selector '{by}'"
                };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

                return wait.Until(d =>
                {
                    var element = d.FindElement(by);
                    return (element.Displayed && element.Enabled) ? element : null;
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"Timed out after waiting for {timeoutInSeconds} seconds to find element with selector '{by}'", ex);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"An error occurred while finding element with selector '{by}'. "
                    + $"Exception: {ex.GetType().Name}, Message: {ex.Message}",
                    ex
                );
            }
        }

        public static void WaitForUrlToMatch(string expectedUrl, int timeoutInSeconds = 10, int pollingIntervalInMilliseconds = 500)
        {
            var timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            var pollingInterval = TimeSpan.FromMilliseconds(pollingIntervalInMilliseconds);
            var wait = new DefaultWait<IWebDriver>(_browser)
            {
                Timeout = timeout,
                PollingInterval = pollingInterval,
                Message = $"Timed out after {timeoutInSeconds} seconds while waiting for URL to match '{expectedUrl}'"
            };

            try
            {
                wait.Until(d => d.Url.Contains(expectedUrl));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new WebDriverTimeoutException($"Timed out after {timeoutInSeconds} seconds while waiting for URL to match '{expectedUrl}'", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while waiting for URL to match '{expectedUrl}'", ex);
            }
        }
    }
}