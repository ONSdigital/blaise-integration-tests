using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class LoginPage
    {
        private readonly By _usernameBox = By.Id("Username");
        private readonly By _passwordBox = By.Id("Password");
        private readonly By _loginButton = By.XPath("//button[@type='submit']");

        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }
        
        public SurveyPage CatiLogin(string username, string password)
        {
            _driver.FindElement(_usernameBox).SendKeys(username);
            _driver.FindElement(_passwordBox).SendKeys(password);
            _driver.FindElement(_loginButton).Click();

            return new SurveyPage(_driver);
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(CatiConfigurationHelper.LoginUrl);
        }
    }
}
