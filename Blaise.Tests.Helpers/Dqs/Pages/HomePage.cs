namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class HomePage : BasePage
    {
        private const string DeployQuestionnaireButtonId = "deploy-questionnaire-link";
        private const string QuestionnaireTableId = "questionnaire-table";
        private const string QuestionnaireTableRowsPath = "//*[@id='questionnaire-table']/tbody/tr";
        private const string SummaryHeadingPath = "//div[contains(@class,'ons-panel')][.//h1[contains(normalize-space(),'deleted successfully')]]//h1";
        private const string InfoButtonPlaceholderId = "info-";
        private const string FilterId = "filter-by-name";

        public HomePage()
            : base(DqsConfigurationHelper.DqsUrl)
        {
        }

        protected override By PageIdentityBy => By.Id(QuestionnaireTableId);

        public void ClickDeployAQuestionnaire()
        {
            ClickButtonById(DeployQuestionnaireButtonId);
        }

        public List<string> GetFirstColumnFromTableContent()
        {
            var elements = GetFirstColumnOfTableFromXPath(QuestionnaireTableRowsPath, QuestionnaireTableId);
            return elements;
        }

        public string GetUploadSummaryText()
        {
            return GetElementTextByPath(SummaryHeadingPath);
        }

        public void ClickQuestionnaireInfoButton(string questionnaireName)
        {
            ClickButtonById(InfoButtonPlaceholderId + questionnaireName);
        }

        public void FilterQuestionnaire(string questionnaireName)
        {
            PopulateInputById(FilterId, questionnaireName);
        }
    }
}
