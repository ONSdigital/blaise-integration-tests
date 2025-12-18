namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Threading;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class CaseInfoPage : BasePage
    {
        private const string QuestionnaireCell = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[1]";
        private const string CaseIdCell = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";
        private const string PlayButton = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a/span";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

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

        public void ClickPlayButton()
        {
            var numberOfWindows = BrowserHelper.GetNumberOfWindows();

            var attempts = 0;
            while (BrowserHelper.GetNumberOfWindows() == numberOfWindows)
            {
                ClickButtonByXPathWithJavaScript(PlayButton);
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
                ClickButtonByXPath(_surveyRadioButton);
                ClickButtonByXPath(ApplyButton);
            }

            ClickButtonByXPath(FilterButton);
        }

        public bool FirstCaseIsPlayable()
        {
            return ElementIsDisplayed(By.XPath(PlayButton));
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return BodyContainsText("Showing");
        }

        private void WaitUntilFirstCaseQuestionnaireIs(string questionnaire)
        {
            WaitUntilElementByXPathContainsText(QuestionnaireCell, questionnaire);
        }

        private void WaitUntilFirstCaseIs(string caseId)
        {
            WaitUntilElementByXPathContainsText(CaseIdCell, caseId);
        }
    }
}
