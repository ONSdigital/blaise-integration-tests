namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Threading;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class SurveyPage : BasePage
    {
        private const string ClearCatiDataButtonPath = @"//*[@id='MVCGridTable_SurveysGrid']/tbody/tr/td[9]/a";
        private const string BackupDataButtonId = "chkBackupAll";
        private const string ClearDataButtonId = "chkClearAll";
        private const string ExecuteButtonPath = "//input[@value='Execute']";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        public SurveyPage()
            : base(CatiConfigurationHelper.SurveyUrl)
        {
        }

        public void ClearDaybatchEntries()
        {
            Thread.Sleep(2000);
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
                ClickButtonByXPath(_surveyRadioButton);
                ClickButtonByXPath(ApplyButton);
            }
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return BodyContainsText("Showing");
        }
    }
}
