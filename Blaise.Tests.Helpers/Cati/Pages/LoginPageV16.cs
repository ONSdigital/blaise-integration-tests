using System;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class LoginPageV16 : BasePage, ILoginPage
    {
        private const string _usernameBoxId = "qa_username";
        private const string _passwordBoxId = "qa_password";
        private const string _loginButtonPath = "//button[@type='submit']";

        public LoginPageV16()
            : base(CatiConfigurationHelper.LoginUrl)
        {
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return driver => driver.FindElement(By.XPath(_loginButtonPath)) != null;
        }

        public void LoginToCati(string username, string password)
        {
            PopulateInputById(_usernameBoxId, username);
            PopulateInputById(_passwordBoxId, password);
            ClickButtonByXPath(_loginButtonPath);
        }
    }
}
