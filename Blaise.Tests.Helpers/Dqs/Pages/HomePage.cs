namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;

    public class HomePage : BasePage
    {
        private const string DeployQuestionnaireButtonId = "deploy-questionnaire-link";
        private const string QuestionnaireTableId = "questionnaire-table";
        private const string QuestionnaireTableRowsPath = "//*[@id='questionnaire-table']/tbody/tr";
        private const string SummaryDivPath = "//div[contains(@class, 'success ons-panel')]";
        private const string InfoButtonPlaceholderId = "info-";
        private const string FilterId = "filter-by-name";

        public HomePage()
            : base(DqsConfigurationHelper.DqsUrl)
        {
        }

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
            return GetElementTextByPath(SummaryDivPath);
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
