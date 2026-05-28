namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using System;
    using System.Linq;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class LoginPage : BasePage
    {
        private readonly string _usernameTextBoxId = "username";
        private readonly string _usernameTextBoxName = "Username";
        private readonly string _passwordTextBoxId = "password";
        private readonly string _passwordTextBoxName = "Password";
        private readonly string _submitButtonPath = "//button[@type='submit']";
        private readonly string _signOutButtonId = "signout-button";
        private readonly string _signOutButtonPath = "//header[contains(@class,'ons-header')]//button[.//span[normalize-space()='Sign out']]";
        private readonly string _signOutButtonCss = "header.ons-header button.ons-btn--link";

        public LoginPage()
            : base(DqsConfigurationHelper.DqsUrl)
        {
        }

        public void LogIntoDqs(string username, string password)
        {
            PopulateLoginInput(_usernameTextBoxId, _usernameTextBoxName, username);
            PopulateLoginInput(_passwordTextBoxId, _passwordTextBoxName, password);
            ClickButtonByXPath(_submitButtonPath);
            WaitUntilLoggedIn();
        }

        public void LogoutOfDqs()
        {
            var signOutButton = BrowserHelper
                .Wait("Timed out waiting for Sign out button")
                .Until(FindSignOutButton);
            signOutButton.Click();
        }

        public bool IsLogoutButtonVisible()
        {
            try
            {
                BrowserHelper
                    .Wait("Timed out waiting for Sign out button", TimeSpan.FromSeconds(5))
                    .Until(driver => FindSignOutButton(driver) != null);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        private void WaitUntilLoggedIn()
        {
            BrowserHelper
                .Wait("Timed out waiting for Sign out button")
                .Until(driver => FindSignOutButton(driver) != null);
        }

        private void PopulateLoginInput(string elementId, string elementName, string value)
        {
            var element = BrowserHelper
                .Wait($"Timed out in PopulateLoginInput(\"{elementId}\", \"{elementName}\")")
                .Until(driver =>
                {
                    var byId = driver.FindElements(By.Id(elementId))
                        .FirstOrDefault(candidate => candidate.Displayed);
                    if (byId != null)
                    {
                        return byId;
                    }

                    return driver.FindElements(By.Name(elementName))
                        .FirstOrDefault(candidate => candidate.Displayed);
                });
            element.Clear();
            element.SendKeys(value);
        }

        private IWebElement FindSignOutButton(IWebDriver driver)
        {
            var byId = driver.FindElements(By.Id(_signOutButtonId))
                .FirstOrDefault(candidate => candidate.Displayed);
            if (byId != null)
            {
                return byId;
            }

            var byXPath = driver.FindElements(By.XPath(_signOutButtonPath))
                .FirstOrDefault(candidate => candidate.Displayed);
            if (byXPath != null)
            {
                return byXPath;
            }

            return driver.FindElements(By.CssSelector(_signOutButtonCss))
                .FirstOrDefault(candidate =>
                    candidate.Displayed &&
                    candidate.Text.IndexOf("Sign out", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        protected override By PageIdentityBy => By.XPath(_submitButtonPath);
    }
}
