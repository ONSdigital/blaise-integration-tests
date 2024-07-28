using System;
using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SpecificationPage : BasePage
    {
        private const string SurveyAccordionPath = "//*[contains(text(), 'Survey Days')]";
        private const string EditButtonId = "btnEditSurveyDays";
        public readonly string TodaysDateInCalenderPickerPath = $"//a[text()='{DateTime.Now.Day}']";
        private const string SaveButtonPath = "//input[@value='Save']";
        private const string QuestionnaireDropDownId = "InstrumentId";

        public SpecificationPage() : base(CatiConfigurationHelper.SpecificationUrl)
        {
        }

        public void SetSurveyDay()
        {
            SelectDropDownValueById(QuestionnaireDropDownId, BlaiseConfigurationHelper.QuestionnaireName);
            Thread.Sleep(3000);
            ClickButtonByXPath(SurveyAccordionPath);
            ClickButtonById(EditButtonId);
            ClickButtonByXPath(TodaysDateInCalenderPickerPath);
            ClickButtonByXPath(SaveButtonPath);
        }
    }
}
