namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class LoginPage : BasePage
    {
        private const string UsernameBoxId = "Username";
        private const string PasswordBoxId = "Password";
        private const string LoginButtonPath = "//button[@type='submit']";

        public LoginPage()
            : base(CatiConfigurationHelper.LoginUrl)
        {
        }

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
