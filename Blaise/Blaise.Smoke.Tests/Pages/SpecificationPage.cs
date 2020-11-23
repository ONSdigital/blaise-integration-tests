﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blaise.Smoke.Tests.Pages
{
    class SpecificationPage
    {
        private IWebDriver _driver;

        private By surveyAccordion = By.XPath("//*[contains(text(), 'Survey Days')]");
        private By editButton = By.Id("btnEditSurveyDays");
        private By calendarDatePicker = By.XPath($"//a[text()='{DateTime.Now.Day}']");
        private By saveButton = By.XPath("//input[@value='Save']");
        private By activeDate = By.ClassName("ui-state-active");

        public SpecificationPage(IWebDriver driver)
        {
            this._driver = driver;
        }

        public void SetSurveyDay()
        {
            Thread.Sleep(1000);
            _driver.FindElement(surveyAccordion).Click();
            Thread.Sleep(1000);
            _driver.FindElement(editButton).Click();
            Thread.Sleep(1000);
            if (_driver.FindElement(activeDate) != null)
            {
                _driver.FindElement(activeDate).Click();
                Thread.Sleep(1000);
            }
            _driver.FindElement(calendarDatePicker).Click();
            _driver.FindElement(saveButton).Click();
        }



        public void GoToPage()
        {
            _driver.Navigate().GoToUrl("https://dev-nik14-web-tel.social-surveys.gcp.onsdigital.uk/Blaise/Specification");
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
