using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Linq;
// ReSharper disable InconsistentNaming

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
            return _browser.PageSource;
        }

        public static void BrowseTo(string pageUrl)
        {
            Browser.Navigate().GoToUrl(pageUrl);
            WaitForUrlToMatch(pageUrl);
        }

        public static void SwitchToLastOpenedWindow()
        {
            var wait = new WebDriverWait(_browser, TimeSpan.FromSeconds(600));
            wait.Until(driver => driver.SwitchTo().Window(Browser.WindowHandles.Last()));
            WaitForUrlToMatch(_browser.Url);
        }

        private static ChromeDriver CreateChromeDriver()
        {
            var chromeOptions = new ChromeOptions
            {
                AcceptInsecureCertificates = true
            };

            chromeOptions.AddArguments("start-maximized");

            return new ChromeDriver(BrowserConfigurationHelper.ChromeDriver, chromeOptions);
        }

        public static IWebElement WaitForElement(By by, bool checkIfClickable = false, int maxAttempts = 50)
        {
            IWebElement element = null;
            var attempts = 0;

            while (element == null && attempts < maxAttempts)
            {
                try
                {
                    element = _browser.FindElement(by);
                }
                catch (NoSuchElementException)
                {
                    // Element not found - continue polling
                }

                if (element == null || !IsElementVisible(element))
                {
                    System.Threading.Thread.Sleep(500);
                }
                else if (!checkIfClickable || IsElementClickable(element))
                {
                    break;
                }
                attempts++;
            }

            if (element == null)
            {
                //Log this error
                throw new NoSuchElementException($"Could not find element by {by} after {maxAttempts} attempts.");
            }

            return element;
        }

        private static bool IsElementVisible(IWebElement element)
        {
            return element.Displayed && element.Enabled;
        }

        private static bool IsElementClickable(IWebElement element)
        {
            try
            {
                if (IsElementVisible(element))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (WebDriverException wdException)
            {
                if (wdException.Message.Contains("Element is not clickable"))
                {
                    return false;
                }
                else
                {
                    throw new Exception($"Error occurred while clicking element: {wdException.Message}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while clicking element: {ex.Message}");
            }
        }
        public static void WaitForUrlToMatch(string url, int timeoutInSeconds = 1200 /*up to 20 minutes*/, double pollingIntervalInSeconds = 0.5)
        {
            try
            {
                var wait = new WebDriverWait(_browser, TimeSpan.FromSeconds(timeoutInSeconds))
                {
                    PollingInterval = TimeSpan.FromSeconds(pollingIntervalInSeconds)
                };
                wait.Until(ExpectedConditions.UrlMatches(url));
            }
            catch (WebDriverTimeoutException wdTOException)
            {
                throw new Exception($"Timed out waiting for URL to match: {url} Exception: {wdTOException}");
            }
            catch (NoSuchElementException nseException)
            {
                throw new Exception($"Error occurred while waiting for URL to match: {url} Exception: {nseException.Message}");
            }
            catch (WebDriverException wdException)
            {
                throw new Exception($"Error occurred while waiting for URL to match: {url} Exception:  {wdException.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while waiting for URL to match: {url} Exception: {ex.Message}");
            }
        }
    }
}
