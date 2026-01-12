using System;
using System.Threading;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class CaseInfoPage : BasePage
    {
        private const string _questionnaireCell = "//*[@id='CaseInfo_content_table']/tbody/tr[1]/td[1]";
        private const string _caseIdCell = "//*[@id='CaseInfo_content_table']/tbody/tr[1]/td[2]";
        private const string _playButton = "qa_startcase_0";
        private const string _filterButton = "//*[contains(text(), 'Filters')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";
        private const string _applyButton = "//*[contains(text(), 'Apply')]";

        public CaseInfoPage()
            : base(CatiConfigurationHelper.CaseInfoUrl)
        {
        }

        public void RefreshPageUntilCaseIsPlayable(string caseId)
        {
            var attempts = 0;
            do
            {
                LoadPage();
                ApplyFilters();
                WaitUntilFirstCaseQuestionnaireIs(BlaiseConfigurationHelper.QuestionnaireName);
                WaitUntilFirstCaseIs(caseId);

                attempts++;
                if (attempts > 5)
                {
                    throw new Exception("Giving up after 5 attempts waiting for play button");
                }
            }
            while (!FirstCaseIsPlayable());
        }

        /// <inheritdoc/>
        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return BodyContainsText("Case Info");
        }

        public void ClickPlayButton()
        {
            var numberOfWindows = BrowserHelper.GetNumberOfWindows();

            var attempts = 0;
            while (BrowserHelper.GetNumberOfWindows() == numberOfWindows)
            {
                ClickButtonById(_playButton);
                Thread.Sleep(250);
                attempts++;
                if (attempts > 5)
                {
                    throw new Exception("Timed out waiting for new window to open.");
                }
            }
        }

        public void ApplyFilters()
        {
            ClickButtonByXPath(_filterButton);
            var filterButtonText = GetElementTextByPath(_filterButton);
            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(_surveyRadioButton);
                ClickButtonByXPath(_applyButton);
            }

            ClickButtonByXPath(_filterButton);
        }

        private void WaitUntilFirstCaseQuestionnaireIs(string questionnaire)
        {
            WaitUntilElementByXPathContainsText(_questionnaireCell, questionnaire);
        }

        private void WaitUntilFirstCaseIs(string caseId)
        {
            WaitUntilElementByXPathContainsText(_caseIdCell, caseId);
        }

        public bool FirstCaseIsPlayable()
        {
            return ElementIsDisplayed(By.Id(_playButton));
        }
    }
}
