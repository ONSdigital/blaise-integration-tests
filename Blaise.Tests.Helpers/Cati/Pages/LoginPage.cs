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
            EnsureCorrectLoginPage();
        }

        private void EnsureCorrectLoginPage()
        {
            try
            {
                Console.WriteLine("Navigating to default login page.");
                Browser.Navigate().GoToUrl(CatiConfigurationHelper.LoginUrl);
                if (BrowserHelper.ElementExistsByXPath("//i[contains(@class, 'bi-bell-fill')]", TimeSpan.FromSeconds(1)))
                {
                    Console.WriteLine("Bell icon detected. Redirecting to NewDashboardLoginUrl.");
                    Browser.Navigate().GoToUrl(CatiConfigurationHelper.NewDashboardLoginUrl);
                }
                else
                {
                    Console.WriteLine("Bell icon not detected. Using default LoginUrl.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while ensuring correct login page: {ex.Message}");
                throw;
            }
        }

        private bool UseNewSelectors
        {
            get
            {
                try
                {
                    return BrowserHelper.ElementExistsByXPath("//i[contains(@class, 'bi-bell-fill')]", TimeSpan.FromSeconds(1));
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
