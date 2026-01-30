namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class LoginPage : BasePage
    {
        private const string LoginButtonPath = "//button[@type='submit']";

        public LoginPage()
            : base(CatiConfigurationHelper.LoginUrl)
        {
        }

        private bool UseNewSelectors
        {
            get
            {
                try
                {
                    return BrowserHelper.ElementExistsById("qa_username");
                }
                catch
                {
                    return false;
                }
            }
        }

        private string UsernameBoxId => UseNewSelectors ? "qa_username" : "Username";
        private string PasswordBoxId => UseNewSelectors ? "qa_password" : "Password";

        public void LoginToCati(string username, string password)
        {
            PopulateInputById(UsernameBoxId, username);
            PopulateInputById(PasswordBoxId, password);
            ClickButtonByXPath(LoginButtonPath);
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return driver => driver.FindElement(By.XPath(LoginButtonPath)) != null;
        }
    }
}
