using System;
using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SpecificationPage : BasePage
    {
        private const string _surveyAccordionPath = "//*[contains(text(), 'Survey Days')]";
        private const string _editButtonId = "btnEditSurveyDays";
        public readonly string TodaysDateInCalenderPickerPath = $"//a[text()='{DateTime.Now.Day}']";
        private const string _saveButtonPath = "//input[@value='Save']";
        private const string _questionnaireDropDownId = "InstrumentId";

        public SpecificationPage()
            : base(CatiConfigurationHelper.SpecificationUrl)
        {
        }

        public void SetSurveyDay()
        {
            SelectDropDownValueById(_questionnaireDropDownId, BlaiseConfigurationHelper.QuestionnaireName);
            Thread.Sleep(3000);
            ClickButtonByXPath(_surveyAccordionPath);
            ClickButtonById(_editButtonId);
            ClickButtonByXPath(TodaysDateInCalenderPickerPath);
            ClickButtonByXPath(_saveButtonPath);
        }
    }
}
