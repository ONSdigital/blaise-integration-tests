using Blaise.Smoke.Tests.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blaise.Smoke.Tests.Pages
{
    class InterviewPage
    {
        private IWebDriver _driver;
        private ConfigurationHelper _configurationHelper;

        private By saveContinueButton = By.Id("q");

        public InterviewPage(IWebDriver driver)
        {
            this._driver = driver;
            this._configurationHelper = new ConfigurationHelper();
        }

        public string GetSaveAndContinueButton()
        {
            Thread.Sleep(3000);
            var n = _driver.FindElement(saveContinueButton);
            return n.Text;
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(_configurationHelper.InterviewURL);
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
