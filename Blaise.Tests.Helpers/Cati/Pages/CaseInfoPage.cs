using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;
using System;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class CaseInfoPage : BasePage
    {
        private string QuestionnaireCell = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[1]";
        private string CaseIDCell = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";
        private string PlayButton = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a/span";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private string SurveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.InstrumentName}']";
        private string ApplyButton = "//*[contains(text(), 'Apply')]";

        public CaseInfoPage() : base(CatiConfigurationHelper.CaseInfoUrl)
        {
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return BodyContainsText("Showing");
        }

        public void ClickPlayButton()
        {
            ClickButtonByXPath(PlayButton);
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

        public void WaitUntilFirstCaseQuestionnaireIs(string questionnaire)
        {
            WaitUntilElementByXPathContainsText(QuestionnaireCell, questionnaire);
        }

        public void WaitUntilFirstCaseIs(string caseId)
        {
            WaitUntilElementByXPathContainsText(CaseIDCell, caseId);
        }

        public bool FirstCaseIsPlayable()
        {
            return ElementIsDisplayed(By.XPath(PlayButton));
        }
    }
}
