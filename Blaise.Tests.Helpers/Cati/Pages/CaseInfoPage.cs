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
        private const string _filterButton = "//*[contains(text(), 'Filters')]";
        private const string _applyButton = "//*[contains(text(), 'Apply')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        public CaseInfoPage()
            : base(CatiConfigurationHelper.CaseInfoUrl)
        {
        }

        private bool UseNewSelectors
        {
            get
            {
                try
                {
                    return BrowserHelper.ElementExistsByXPath("//i[contains(@class, 'bi-bell-fill')]");
                }
                catch
                {
                    return false;
                }
            }
        }

        private string QuestionnaireCellPath => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']/tbody/tr[1]/td[1]"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[1]";

        private string CaseIdCellPath => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']/tbody/tr[1]/td[2]"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";

        private string PlayButtonSelector => UseNewSelectors
            ? "qa_startcase_0" // ID for new version
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a/span"; // XPath for old version

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
                if (UseNewSelectors)
                {
                    ClickButtonById(PlayButtonSelector);
                }
                else
                {
                    ClickButtonByXPathWithJavaScript(PlayButtonSelector);
                }

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

        public bool FirstCaseIsPlayable()
        {
            return UseNewSelectors
                ? ElementIsDisplayed(By.Id(PlayButtonSelector))
                : ElementIsDisplayed(By.XPath(PlayButtonSelector));
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return UseNewSelectors
                ? BodyContainsText("Case Info")
                : BodyContainsText("Showing");
        }

        private void WaitUntilFirstCaseQuestionnaireIs(string questionnaire)
        {
            WaitUntilElementByXPathContainsText(QuestionnaireCellPath, questionnaire);
        }

        private void WaitUntilFirstCaseIs(string caseId)
        {
            WaitUntilElementByXPathContainsText(CaseIdCellPath, caseId);
        }
    }
}
