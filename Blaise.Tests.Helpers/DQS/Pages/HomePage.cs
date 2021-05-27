using System.Collections.Generic;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class HomePage : BasePage
    {
        private const string DeployQuestionnaireButtonId = "deploy-questionnaire-link";
        public string QuestionnaireTableId = "instrument-table";
        public string QuestionnaireTableRowsPath = "//*[@id='instrument-table']/tbody/tr";
        public string DeletePlaceholderId = "delete-";
        public string DeleteButtonId = "delete-button-";
        private const string SummaryDivPath = "//div[contains(@class, 'success panel')]";

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

        public void CheckForDeleteButton(string instrumentName)
        {
            ButtonIsAvailableById(DeletePlaceholderId + instrumentName);
        }

        public void ClickDeleteButton(string instrumentName)
        {
            ClickButtonById(DeleteButtonId + instrumentName);
        }

        public string GetDeleteColumnText(string instrumentName)
        {
            return GetElementTextById(DeletePlaceholderId + instrumentName);
        }

        public string GetUploadSummaryText()
        {
            return GetElementTextByPath(SummaryDivPath);
        }
    }
}
