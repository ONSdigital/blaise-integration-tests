using System;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SpecificationPage : BasePage
    {
        private const string SurveyAccordionPath = "//*[contains(text(), 'Survey Days')]";
        private const string EditButtonId = "btnEditSurveyDays";
        public readonly string TodaysDateInCalenderPickerPath = $"//a[text()='{DateTime.Now.Day}']";
        private const string SaveButtonPath = "//input[@value='Save']";
        private const string InstrumentDropdownByPath = "//*[@id='InstrumentId']";
        private const string RefreshButtonId = "btnRefreshSpecification";
        private const string ToastSuccessClass = "toast-success";

        public SpecificationPage() : base(CatiConfigurationHelper.SpecificationUrl)
        {
        }

        public void SetSurveyDay(string instrumentName)
        {
            SelectDropDownListItem(InstrumentDropdownByPath, instrumentName);
            ClickButtonById(RefreshButtonId);
            SuccessToastByClass(ToastSuccessClass);
            ClickButtonByXPath(SurveyAccordionPath);
            ClickButtonById(EditButtonId);
            ClickButtonByXPath(TodaysDateInCalenderPickerPath);
            ClickButtonByXPath(SaveButtonPath);
        }
    }
}
