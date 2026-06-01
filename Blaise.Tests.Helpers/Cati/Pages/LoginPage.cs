namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class LoginPage : BasePage
    {
        private const string LoginButtonPath = "//button[@type='submit']";
        private string _loginUrl;

        public LoginPage()
            : base(CatiConfigurationHelper.LoginUrl)
        {
            EnsureCorrectLoginPage();
        }

        protected override By PageIdentityBy => By.XPath(LoginButtonPath);

        private bool UseNewSelectors => CatiUiVersionHelper.IsNewUi;

        private string UsernameBoxId => UseNewSelectors ? "qa_username" : "Username";

        private string PasswordBoxId => UseNewSelectors ? "qa_password" : "Password";

        public void LoginToCati(string username, string password)
        {
            PopulateInputById(UsernameBoxId, username);
            PopulateInputById(PasswordBoxId, password);
            ClickButtonByXPath(LoginButtonPath);
        }

        public void LoadLoginPage()
        {
            if (string.IsNullOrWhiteSpace(_loginUrl))
            {
                EnsureCorrectLoginPage();
            }

            BrowserHelper.BrowseTo(_loginUrl);
            BrowserHelper.Wait($"Timed out waiting for CATI login page {_loginUrl} to load")
                .Until(PageHasLoaded());
        }

        private void EnsureCorrectLoginPage()
        {
            try
            {
                CatiUiVersionHelper.DetectAndCache();
                _loginUrl = CatiUiVersionHelper.IsNewUi
                    ? CatiConfigurationHelper.NewDashboardLoginUrl
                    : CatiConfigurationHelper.LoginUrl;
                Console.WriteLine($"Using CATI login page: {_loginUrl}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while ensuring correct login page: {ex.Message}");
                throw;
            }
        }
    }
}
