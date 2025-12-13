namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Threading;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;

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
            Thread.Sleep(3000);
            ClickButtonByXPath(SurveyAccordionPath);
            ClickButtonById(EditButtonId);
            ClickButtonByXPath(_todaysDateInCalenderPickerPath);
            ClickButtonByXPath(SaveButtonPath);
        }
    }
}
