namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;

    public class HomePage : BasePage
    {
        private const string _deployQuestionnaireButtonId = "deploy-questionnaire-link";
        private const string _questionnaireTableId = "questionnaire-table";
        private const string _questionnaireTableRowsPath = "//*[@id='questionnaire-table']/tbody/tr";
        private const string _summaryDivPath = "//div[contains(@class, 'success ons-panel')]";
        private const string _infoButtonPlaceholderId = "info-";
        private const string _filterId = "filter-by-name";

        public HomePage()
            : base(DqsConfigurationHelper.DqsUrl)
        {
        }

        public void ClickDeployAQuestionnaire()
        {
            ClickButtonById(_deployQuestionnaireButtonId);
        }

        public List<string> GetFirstColumnFromTableContent()
        {
            var elements = GetFirstColumnOfTableFromXPath(_questionnaireTableRowsPath, _questionnaireTableId);
            return elements;
        }

        public string GetUploadSummaryText()
        {
            return GetElementTextByPath(_summaryDivPath);
        }

        public void ClickQuestionnaireInfoButton(string questionnaireName)
        {
            ClickButtonById(_infoButtonPlaceholderId + questionnaireName);
        }

        public void FilterQuestionnaire(string questionnaireName)
        {
            PopulateInputById(_filterId, questionnaireName);
        }
    }
}
