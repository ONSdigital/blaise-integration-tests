using Blaise.Smoke.Tests.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Smoke.Tests.Pages
{
    class LoginPage
    {
        private IWebDriver _driver;
        private WebDriverWait wait;
        private ConfigurationHelper _configurationHelper;

        public LoginPage(IWebDriver driver)
        {
            this._driver = driver;
            this._configurationHelper = new ConfigurationHelper();
        }

        private readonly By usernameBox = By.Id("Username");

        private By passwordBox = By.Id("Password");

        private By loginButton = By.XPath("//button[@type='submit']");

        public SurveyPage CatiLogin(String username, string password)
        {
            _driver.FindElement(usernameBox).SendKeys(username);
            _driver.FindElement(passwordBox).SendKeys(password);
            _driver.FindElement(loginButton).Click();
            return new SurveyPage(_driver);
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(_configurationHelper.LoginURL);
        }
    }
}
