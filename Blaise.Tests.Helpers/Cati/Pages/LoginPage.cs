using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class LoginPage : BasePage, ILoginPage
    {
        private const string _usernameBoxId = "Username";
        private const string _passwordBoxId = "Password";
        private const string _loginButtonPath = "//button[@type='submit']";

        public LoginPage()
            : base(CatiConfigurationHelper.LoginUrl)
        {
        }

        /// <summary>
        /// Logs into the CATI dashboard using the specified username and password.
        /// </summary>
        /// <param name="username">The username to use for login.</param>
        /// <param name="password">The password to use for login.</param>
        public void LoginToCati(string username, string password)
        {
            PopulateInputById(_usernameBoxId, username);
            PopulateInputById(_passwordBoxId, password);
            ClickButtonByXPath(_loginButtonPath);
        }
    }
}
