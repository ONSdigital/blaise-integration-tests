using System;
using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Unity.Injection;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SpecificationPage : BasePage
    {
        private readonly string _surveyAccordionPath = "//*[contains(text(), 'Survey Days')]";
        private readonly string _editButtonId = "btnEditSurveyDays";
        private readonly string _todaysDateInCalenderPickerPath = $"//a[text()='{DateTime.Now.Day}']";
        private readonly string _saveButtonPath = "//input[@value='Save']";

        public SpecificationPage() : base(CatiConfigurationHelper.SpecificationUrl)
        {
        }

        public void SetSurveyDay()
        {
            ClickButtonByXPath(_surveyAccordionPath);
            ClickButtonById(_editButtonId);
            ClickButtonByXPath(_todaysDateInCalenderPickerPath);
            ClickButtonByXPath(_saveButtonPath);
        }
    }
}
