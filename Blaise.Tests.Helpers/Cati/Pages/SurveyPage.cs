using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;
using System.Xml.Linq;
// ReSharper disable InconsistentNaming

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SurveyPage : BasePage
    {
        private const string ClearCatiDataButtonId = @"qa_backupdata_0";
        private const string BackupAndClearCatiDataButtonPath = "//button[contains(@id, 'qa_backupdata_')]";
        private const string BackupDataButtonPath = "//div[@class='row'][2]//input[@type='checkbox'][1]";
        private const string ClearDataButtonPath = "//input[@id='qa_clear_all']";
        private const string ExecuteButtonPath = "//input[@value='Execute']";
        private const string FilterButton = "//*[contains(text(), 'Database Filters')]";
        private readonly string SurveyTextFieldId = "delim_val_qa_filter_surveymultiple"; 
        private readonly string SurveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";
        private const string ApplyButtonId = "qa_filter_apply";

        public SurveyPage() : base(CatiConfigurationHelper.SurveyUrl)
        {
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return BodyContainsText("Surveys");
        }

        public void ClearDayBatchEntries()
        {
            Thread.Sleep(2000);
            ClickButtonByXPath(BackupAndClearCatiDataButtonPath); // Working

            Thread.Sleep(2000);
            var element = BrowserHelper.SwitchToActiveElement();
            // modal-dialog-5825ca1f-b235-4b0e-ab94-562d44a9d0c9
            Thread.Sleep(2000);
            element.SendKeys(Keys.Space);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Space);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Tab);
            element.SendKeys(Keys.Space);

            //ClickButtonByXPath(BackupDataButtonPath);
            //ClickButtonById(ClearCatiDataButtonId);

            //ClickButtonById(ClearDataButtonId);
            //ClickButtonByXPath(ExecuteButtonPath);
        }

        public void ApplyFilter()
        {
            Thread.Sleep(2000);
            ClickButtonByXPath(FilterButton);
            ClickButtonById("qa_filter_surveymultiple");
            var element = BrowserHelper.SwitchToActiveElement();
            element.SendKeys(Keys.Space);
            ClickButtonById(ApplyButtonId);
        }
    }
}
