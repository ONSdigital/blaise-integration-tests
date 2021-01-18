using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using System.Collections.Generic;

namespace Blaise.Tests.Helpers.DQS.Pages
{
    public class HomePage : BasePage
    {
        private readonly string deployQuestionnaireButtonID = "deploy-questionnaire-link";
        public string QuestionnaireTableId = "instrument-table";
        public string QuestionnaireTableRowsPath = "//*[@id='instrument-table']/tbody/tr";

        public HomePage() : base(DqsConfigurationHelper.DqsUrl)
        {
        }

        public void ClickDeployAQuestionnaire()
        {
            ClickButtonById(deployQuestionnaireButtonID);
        }

        public List<string> GetTableContent()
        {
            var elements = GetFirstColumnOfTableFromXPath(QuestionnaireTableRowsPath, QuestionnaireTableId);
            return elements;
        }
    }
}
