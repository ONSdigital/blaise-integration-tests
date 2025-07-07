using System;
using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;

// ReSharper disable InconsistentNaming
namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SurveyPage : BasePage
    {
        private const string _clearCatiDataButtonPath = @"//*[@id='MVCGridTable_SurveysGrid']/tbody/tr/td[9]/a";
        private const string _backupDataButtonId = "chkBackupAll";
        private const string _clearDataButtonId = "chkClearAll";
        private const string _executeButtonPath = "//input[@value='Execute']";
        private const string _filterButton = "//*[contains(text(), 'Filters')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";
        private const string _applyButton = "//*[contains(text(), 'Apply')]";

        public SurveyPage()
            : base(CatiConfigurationHelper.SurveyUrl)
        {
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return BodyContainsText("Showing");
        }

        public void ClearDayBatchEntries()
        {
            Thread.Sleep(2000);
            ClickButtonByXPath(_clearCatiDataButtonPath);
            ClickButtonById(_backupDataButtonId);
            ClickButtonById(_clearDataButtonId);
            ClickButtonByXPath(_executeButtonPath);
        }

        public void ApplyFilter()
        {
            ClickButtonByXPath(_filterButton);
            var filterButtonText = GetElementTextByPath(_filterButton);
            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(_surveyRadioButton);
                ClickButtonByXPath(_applyButton);
            }
        }
    }
}
