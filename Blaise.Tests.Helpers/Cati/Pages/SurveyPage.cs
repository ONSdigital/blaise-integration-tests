using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;
using System;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SurveyPage : BasePage
    {
        private const string ClearCatiDataButtonPath = @"//*[@id='MVCGridTable_SurveysGrid']/tbody/tr/td[9]/a";
        private const string BackupDataButtonId = "chkBackupAll";
        private const string ClearDataButtonId = "chkClearAll";
        private const string ExecuteButtonPath = "//input[@value='Execute']";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private string SurveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.InstrumentName}']";
        private string ApplyButton = $"//*[contains(text(), 'Apply')]";

        public SurveyPage() : base(CatiConfigurationHelper.SurveyUrl)
        {
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return BodyContainsText("Showing");
        }

        public void ClearDayBatchEntries()
        {
            ClickButtonByXPath(ClearCatiDataButtonPath);
            ClickButtonById(BackupDataButtonId);
            ClickButtonById(ClearDataButtonId);
            ClickButtonByXPath(ExecuteButtonPath);
        }

        public void ApplyFilter()
        {
            ClickButtonByXPath(FilterButton);
            var filterButtonText = GetElementTextByPath(FilterButton);
            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(SurveyRadioButton);
                ClickButtonByXPath(ApplyButton);
            }
        }
    }
}
