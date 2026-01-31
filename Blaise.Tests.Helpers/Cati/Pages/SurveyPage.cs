namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Threading;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class SurveyPage : BasePage
    {
        private const string _filterButton = "//*[contains(text(), 'Filters')]";
        private const string _applyButton = "//*[contains(text(), 'Apply')]";

        private bool UseNewSelectors
        {
            get
            {
                try
                {
                    return BrowserHelper.ElementExistsByXPath("//i[contains(@class, 'bi-bell-fill')]", TimeSpan.FromSeconds(1));
                }
                catch
                {
                    return false;
                }
            }
        }

        private string ClearCatiDataButtonPath => UseNewSelectors
            ? "//*[contains(text(), 'Clear')]"
            : "//*[@id='MVCGridTable_SurveysGrid']/tbody/tr/td[9]/a";

        private string BackupDataButtonId => UseNewSelectors
            ? "qa_backupdata_0"
            : "chkBackupAll";

        private string ClearDataButtonSelector => UseNewSelectors
            ? "//label[@for='qa_clear_all']"
            : "chkClearAll";

        private string ExecuteButtonPath => UseNewSelectors
            ? "//button[@id='qa_btn_submit']"
            : "//input[@value='Execute']";

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

            if (UseNewSelectors)
            {
                ClickButtonByXPath(ClearDataButtonSelector);
            }
            else
            {
                ClickButtonById(ClearDataButtonSelector);
            }

            ClickButtonByXPath(ExecuteButtonPath);
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

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return UseNewSelectors
                ? BodyContainsText("Surveys")
                : BodyContainsText("Showing");
        }
    }
}
