using System;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SurveyPage
    {
        private readonly IWebDriver _driver;

        public SurveyPage(IWebDriver driver)
        {
            _driver = driver;
            
            if (_driver.Title != ("Surveys - CATI Dashboard"))
            {
                throw new Exception("This is not the Survey Page of logged in user," +
                      " current page is: " + _driver.Url);
            }
        }

        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(CatiConfigurationHelper.SurveyUrl);
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
