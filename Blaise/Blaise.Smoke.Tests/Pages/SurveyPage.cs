using Blaise.Smoke.Tests.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Smoke.Tests.Pages
{
    class SurveyPage
    {
        private IWebDriver _driver;
        private ConfigurationHelper _configurationHelper;

        public SurveyPage(IWebDriver driver)
        {
            this._driver = driver;
            if (_driver.Title != ("Surveys - CATI Dashboard"))
            {
                throw new Exception("This is not the Survey Page of logged in user," +
                      " current page is: " + _driver.Url);
            }
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(_configurationHelper.SurveyURL);
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
