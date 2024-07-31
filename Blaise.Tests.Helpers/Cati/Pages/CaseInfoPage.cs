using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;
using System;
using System.Threading;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class CaseInfoPage : BasePage
    {
        private const string QuestionnaireCell = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[1]";
        private const string CaseIdCell = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";
        private const string PlayButton = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a/span";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private readonly string SurveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";

        public CaseInfoPage() : base(CatiConfigurationHelper.CaseInfoUrl)
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
            } while (!FirstCaseIsPlayable());
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return BodyContainsText("Showing");
        }

        public void ClickPlayButton()
        {
            var numberOfWindows = BrowserHelper.GetNumberOfWindows();

            var attempts = 0;
            while (BrowserHelper.GetNumberOfWindows() == numberOfWindows)
            {
                ClickButtonByXPath(PlayButton);
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
            ClickButtonByXPath(FilterButton);
            var filterButtonText = GetElementTextByPath(FilterButton);
            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(SurveyRadioButton);
                ClickButtonByXPath(ApplyButton);
            }
            ClickButtonByXPath(FilterButton);
        }

        private void WaitUntilFirstCaseQuestionnaireIs(string questionnaire)
        {
            WaitUntilElementByXPathContainsText(QuestionnaireCell, questionnaire);
        }

        private void WaitUntilFirstCaseIs(string caseId)
        {
            WaitUntilElementByXPathContainsText(CaseIdCell, caseId);
        }

        public bool FirstCaseIsPlayable()
        {
            return ElementIsDisplayed(By.XPath(PlayButton));
        }
    }
}
