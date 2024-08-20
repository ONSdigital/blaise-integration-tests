using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using System.Collections.Generic;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class HomePage : BasePage
    {
        private const string DeployQuestionnaireButtonId = "deploy-questionnaire-link";
        public string QuestionnaireTableId = "questionnaire-table";
        public string QuestionnaireTableRowsPath = "//*[@id='questionnaire-table']/tbody/tr";
        private const string SummaryDivPath = "//div[contains(@class, 'success ons-panel')]";
        public string InfoButtonPlaceholderId = "info-";
        public string FilterId = "filter-by-name";

        public HomePage() : base(DqsConfigurationHelper.DqsUrl)
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
