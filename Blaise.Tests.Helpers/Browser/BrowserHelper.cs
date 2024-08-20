using Blaise.Tests.Helpers.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Blaise.Tests.Helpers.Browser
{
    public static class BrowserHelper
    {
        private static IWebDriver _browser;
        private static IWebDriver Browser => _browser ?? (_browser = CreateChromeDriver());
        public static int TimeOutInSeconds => BrowserConfigurationHelper.TimeOutInSeconds;
        public static string CurrentUrl => Browser.Url;

        public static WebDriverWait Wait(string message)
        {
            return new WebDriverWait(Browser, TimeSpan.FromSeconds(TimeOutInSeconds))
            {
                Message = message
            };
        }

        public static WebDriverWait Wait(string message, TimeSpan? timeout = null)
        {
            return new WebDriverWait(Browser, timeout ?? TimeSpan.FromSeconds(TimeOutInSeconds))
            {
                Message = message
            };
        }

        public static string GetCurrentUrl()
        {
            return _browser.Url;
        }

        public static void ClosePreviousTab()
        {
            if (_browser == null) return;
            var tabs = _browser.WindowHandles;
            if (tabs.Count > 1)
            {
                _browser.SwitchTo().Window(tabs[0]);
                _browser.Close();
                _browser.SwitchTo().Window(tabs[1]);
            }
        }

        public static void PopulateInputByName(string elementName, string value)
        {
            var wait = new WebDriverWait(_browser, TimeSpan.FromSeconds(TimeOutInSeconds));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.Name(elementName)));
            try
            {
                // Enter the value into the element
                element.SendKeys(value);
            }
            catch (StaleElementReferenceException)
            {
                // Element has become stale, re-find the element and retry sending keys
                element = wait.Until(ExpectedConditions.ElementIsVisible(By.Name(elementName)));
                element.SendKeys(value);
            }
        }

        public static void ClearSessionData()
        {
            if (_browser == null) return;
            _browser.Manage().Cookies.DeleteAllCookies();
            var wait = new WebDriverWait(_browser, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.Manage().Cookies.AllCookies.Count == 0);
            var jsExecutor = (IJavaScriptExecutor)_browser;
            jsExecutor.ExecuteScript("window.localStorage.clear();");
            jsExecutor.ExecuteScript("window.sessionStorage.clear();");
            _browser.Quit();
        }

        public static bool ElementExistsById(string id, TimeSpan? timeout = null)
        {
            try
            {
                Wait($"Timed out in ElementExistsById(\"{id}\")", timeout)
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(id)));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public static bool ElementExistsByXPath(string xPath)
        {
            return Browser.FindElements(By.XPath(xPath)).Count > 0;
        }

        public static void ScrollHorizontalByOffset(int offset)
        {
            var actions = new Actions(Browser);
            actions.MoveByOffset(offset, 0);
            actions.Perform();
        }

        public static void ScrollIntoView(IWebElement element)
        {
            Browser.ExecuteJavaScript("arguments[0].scrollIntoView(true);", element);
        }

        public static string TakeScreenShot(string screenShotPath, string screenShotName)
        {
            try
            {
                var screenShot = _browser.TakeScreenshot();
                var screenShotFile = Path.Combine(screenShotPath, $"{screenShotName}.png");
                screenShot.SaveAsFile(screenShotFile);
                return screenShotFile;
            }
            catch (WebDriverException ex)
            {
                Console.WriteLine($"Error taking screenshot: {ex.Message}");
                return null;
            }
        }

        public static void SaveAndAttachHtml(NUnit.Framework.TestContext testContext, string baseFileName)
        {
            try
            {
                if (testContext == null)
                {
                    Console.WriteLine("TestContext is null. Cannot save or attach HTML.");
                    return;
                }

                if (string.IsNullOrEmpty(baseFileName))
                {
                    Console.WriteLine("BaseFileName is null or empty. Using default name.");
                    baseFileName = "default_html_capture";
                }

                string htmlFileName = $"{baseFileName}.html";
                string htmlFilePath = Path.Combine(testContext.WorkDirectory, htmlFileName);
                string htmlContent = CurrentWindowHtml();

                if (string.IsNullOrEmpty(htmlContent))
                {
                    Console.WriteLine("HTML content is empty. Not saving or attaching.");
                    return;
                }

                File.WriteAllText(htmlFilePath, htmlContent);
                TestContext.AddTestAttachment(htmlFilePath, "Window HTML");
                Console.WriteLine($"HTML saved and attached: {htmlFilePath}");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"NullReferenceException in SaveAndAttachHtml: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save or attach HTML: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        public static void OnError(NUnit.Framework.TestContext testContext, ScenarioContext scenarioContext)
        {
            if (scenarioContext.ContainsValue(scenarioContext.StepContext.StepInfo.Text))
                return;

            string baseFileName = scenarioContext.StepContext.StepInfo.Text;

            if (_browser!= null)
            {
                var screenShotFile = TakeScreenShot(testContext.WorkDirectory, baseFileName);
                
                if (screenShotFile!= null)
                {
                    TestContext.AddTestAttachment(screenShotFile, baseFileName);
                }
                else
                {
                    Console.WriteLine("Unable to take screenshot for error reporting.");
                }

                SaveAndAttachHtml(testContext, baseFileName);
            }
            else
            {
                Console.WriteLine("Browser was not initialised when error occurred.");
            }

            scenarioContext.Remove("Error");
            scenarioContext.Add("Error", scenarioContext.TestError.Message);
            scenarioContext.ScenarioContainer.RegisterInstanceAs(scenarioContext.TestError);
        }

        public static void CloseBrowser()
        {
            Browser.Quit();
            _browser = null;
        }

        public static string CurrentWindowHtml()
        {
            try
            {
                if (_browser == null)
                {
                    Console.WriteLine("Browser instance is null. Cannot get HTML.");
                    return null;
                }

                string html = _browser.PageSource;

                if (string.IsNullOrEmpty(html))
                {
                    Console.WriteLine("Retrieved HTML is null or empty.");
                    return null;
                }

                return html;
            }
            catch (WebDriverException ex)
            {
                Console.WriteLine($"WebDriverException in CurrentWindowHtml: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CurrentWindowHtml: {ex.Message}");
                return null;
            }
        }

        public static void BrowseTo(string pageUrl)
        {
            Browser.Navigate().GoToUrl(pageUrl);
        }

        public static int GetNumberOfWindows()
        {
            return Browser.WindowHandles.Count;
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
            chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments("start-maximized");
            return new ChromeDriver(BrowserConfigurationHelper.ChromeDriver, chromeOptions);
        }

        public static void WaitForTextInHtml(string text)
        {
            Wait($"Timed out in WaitForTextInHtml(\"{text}\")")
                .Until(driver => CurrentWindowHtml().Contains(text));
        }

        public static void WaitForElementByXPath(string xPath)
        {
            FindElement(By.XPath(xPath));
        }

        public static IWebElement FindElement(By by)
        {
            return Wait($"Timed out in FindElement({by})'")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public static IReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Wait($"Timed out in FindElements({by})")
                .Until(driver => driver.FindElements(by));
        }

        public static bool ElementIsDisplayed(By by)
        {
            try
            {
                return Browser.FindElement(by).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
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
