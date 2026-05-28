namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class SpecificationPage : BasePage
    {
        private const string SurveyAccordionPath = "//*[contains(text(), 'Survey Days')]";
        private const string EditButtonId = "btnEditSurveyDays";
        private const string SaveButtonPath = "//input[@value='Save']";
        private const string QuestionnaireDropDownId = "InstrumentId";
        private readonly string _todaysDateInCalenderPickerPath = $"//a[text()='{DateTime.Now.Day}']";

        public SpecificationPage()
            : base(CatiConfigurationHelper.SpecificationUrl)
        {
        }

        public void SetSurveyDay()
        {
            SelectDropDownValueById(QuestionnaireDropDownId, BlaiseConfigurationHelper.QuestionnaireName);
            ClickButtonByXPath(SurveyAccordionPath);
            ClickButtonById(EditButtonId);
            ClickButtonByXPath(_todaysDateInCalenderPickerPath);
            ClickButtonByXPath(SaveButtonPath);
        }

        protected override By PageIdentityBy => By.XPath(SurveyAccordionPath);
    }
}
