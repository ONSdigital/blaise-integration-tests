using System;
using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SpecificationPage
    {
        private readonly By _surveyAccordion = By.XPath("//*[contains(text(), 'Survey Days')]");
        private readonly By _editButton = By.Id("btnEditSurveyDays");
        private readonly By _calendarDatePicker = By.XPath($"//a[text()='{DateTime.Now.Day}']");
        private readonly By _saveButton = By.XPath("//input[@value='Save']");
        private readonly By _activeDate = By.ClassName("ui-state-active");

        private readonly IWebDriver _driver;

        public SpecificationPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void SetSurveyDay()
        {
            Thread.Sleep(1000);

            _driver.FindElement(_surveyAccordion).Click();
            Thread.Sleep(1000);

            _driver.FindElement(_editButton).Click();
            Thread.Sleep(1000);

            if (_driver.FindElement(_activeDate) != null)
            {
                _driver.FindElement(_activeDate).Click();
                Thread.Sleep(1000);
            }

            _driver.FindElement(_calendarDatePicker).Click();
            _driver.FindElement(_saveButton).Click();
        }
        
        public void GoToPage()
        {
            _driver.Navigate().GoToUrl(CatiConfigurationHelper.SpecificationUrl);
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
