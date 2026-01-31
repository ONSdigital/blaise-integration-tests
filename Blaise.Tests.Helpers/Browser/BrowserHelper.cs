namespace Blaise.Tests.Helpers.Browser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Blaise.Tests.Helpers.Configuration;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.Extensions;
    using OpenQA.Selenium.Support.UI;
    using Reqnroll;
    using SeleniumExtras.WaitHelpers;

    public static class BrowserHelper
    {
        private static IWebDriver _browser;

        public static int TimeOutInSeconds => BrowserConfigurationHelper.TimeOutInSeconds;

        public static string CurrentUrl => Browser.Url;

        private static IWebDriver Browser => _browser ?? (_browser = CreateChromeDriver());

        public static WebDriverWait Wait(string message)
        {
            return new WebDriverWait(Browser, TimeSpan.FromSeconds(TimeOutInSeconds))
            {
                Message = message,
            };
        }

        public static WebDriverWait Wait(string message, TimeSpan? timeout = null)
        {
            return new WebDriverWait(Browser, timeout ?? TimeSpan.FromSeconds(TimeOutInSeconds))
            {
                Message = message,
            };
        }

        public static string GetCurrentUrl()
        {
            return _browser.Url;
        }

        public static void ClosePreviousTab()
        {
            if (_browser == null)
            {
                return;
            }

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
                element.SendKeys(value);
            }
            catch (StaleElementReferenceException)
            {
                // element has become stale, re-find the element and retry sending keys
                element = wait.Until(ExpectedConditions.ElementIsVisible(By.Name(elementName)));
                element.SendKeys(value);
            }
        }

        public static void ClearSessionData()
        {
            if (_browser == null)
            {
                return;
            }

            _browser.Manage().Cookies.DeleteAllCookies();
            var wait = new WebDriverWait(_browser, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.Manage().Cookies.AllCookies.Count == 0);
            var jsExecutor = (IJavaScriptExecutor)_browser;
            jsExecutor.ExecuteScript("window.localStorage.clear();");
            jsExecutor.ExecuteScript("window.sessionStorage.clear();");
            _browser.Quit();
            _browser = null;
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

        public static bool ElementExistsByXPath(string xPath, TimeSpan? timeout = null)
        {
            try
            {
                Wait($"Timed out in ElementExistsByXPath(\"{xPath}\")", timeout)
                    .Until(ExpectedConditions.ElementExists(By.XPath(xPath)));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public static void ScrollIntoView(IWebElement element)
        {
            new Actions(Browser).MoveToElement(element).Perform();
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
                Console.WriteLine($"NullReferenceException in SaveAndAttachHtml. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save or attach HTML. Error: {ex.Message}");
            }
        }

        public static void OnError(NUnit.Framework.TestContext testContext, ScenarioContext scenarioContext)
        {
            if (scenarioContext.ContainsKey("ErrorHandled"))
            {
                return;
            }

            string stepText = scenarioContext.StepContext.StepInfo.Text;
            string baseFileName = new string(stepText.Where(character => !Path.GetInvalidFileNameChars().Contains(character)).ToArray());
            baseFileName = baseFileName.Length > 100 ? baseFileName.Substring(0, 100) : baseFileName;

            if (_browser != null)
            {
                var screenShotFile = TakeScreenShot(testContext.WorkDirectory, baseFileName);

                if (screenShotFile != null)
                {
                    TestContext.AddTestAttachment(screenShotFile, baseFileName);
                }
                else
                {
                    Console.WriteLine("Unable to take screenshot for error reporting");
                }

                SaveAndAttachHtml(testContext, baseFileName);
            }
            else
            {
                Console.WriteLine("Browser was not initialised when error occurred");
            }

            scenarioContext.Add("ErrorHandled", true);
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
                    Console.WriteLine("Retrieved HTML is null or empty");
                    return null;
                }

                return html;
            }
            catch (WebDriverException ex)
            {
                Console.WriteLine($"WebDriverException in CurrentWindowHtml. Error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CurrentWindowHtml. Error: {ex.Message}");
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
            return Wait($"Timed out in FindElement({by})")
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public static void ScrollIntoViewAndClick(By by)
        {
            var element = FindElement(by);
            ScrollIntoView(element);
            element.Click();
        }

        public static void ScrollIntoViewAndClickById(string id)
        {
            ScrollIntoViewAndClick(By.Id(id));
        }

        public static void ClickWithJavaScript(By by)
        {
            var element = FindElement(by);
            var js = (IJavaScriptExecutor)Browser;
            js.ExecuteScript("arguments[0].click()", element);
        }

        private static void ClickWithRetry(By by)
        {
            try
            {
                Wait($"Timed out in ClickWithRetry({by})")
                    .Until(ExpectedConditions.ElementToBeClickable(by)).Click();
            }
            catch (StaleElementReferenceException)
            {
                Wait($"Timed out in ClickWithRetry({by}) after stale element")
                    .Until(ExpectedConditions.ElementToBeClickable(by)).Click();
            }
        }

        public static void ClickByIdWithRetry(string id)
        {
            ClickWithRetry(By.Id(id));
        }

        public static void ClickByXPathWithRetry(string xpath)
        {
            ClickWithRetry(By.XPath(xpath));
        }

        public static IReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Wait($"Timed out in FindElements({by})")
                .Until(driver =>
                {
                    var elements = driver.FindElements(by);
                    return elements.Count > 0 ? elements : null;
                });
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
                Message = $"Timed out after {timeoutInSeconds} seconds while waiting for URL to match '{expectedUrl}'",
            };

            try
            {
                wait.Until(d => d.Url.Contains(expectedUrl));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new WebDriverTimeoutException($"Timed out after {timeoutInSeconds} seconds while waiting for URL to match '{expectedUrl}'. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while waiting for URL to match '{expectedUrl}'. Error: {ex.Message}");
            }
        }

        private static ChromeDriver CreateChromeDriver()
        {
            var chromeOptions = new ChromeOptions
            {
                AcceptInsecureCertificates = true,
            };
            chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments("start-maximized");
            chromeOptions.AddArguments("--ignore-certificate-errors");
            return new ChromeDriver(chromeOptions);
        }
    }
}
