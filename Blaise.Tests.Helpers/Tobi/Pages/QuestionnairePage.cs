using System.Collections.Generic;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Tobi.Pages
{
    public class QuestionnairePage : BasePage
    {
        public string QuestionnaireTableId = "instrument-table";
        public string QuestionnaireTableRowsPath = "//*[@id='instrument-table']/tbody/tr";
        public string ReturnSurveyId = "return-to-survey-list";

        public QuestionnairePage()
            : base(TobiConfigurationHelper.SurveyUrl)
        {
        }

        public void ClickInterviewButton(string questionnaire)
        {
            var questionnaireIndex = GetTableContent().FindIndex(s => s.Contains(questionnaire)) + 1;
            var interviewLinkPath = $"{QuestionnaireTableRowsPath}[{questionnaireIndex}]/td[3]/a";
            ClickButtonByXPath(interviewLinkPath);
            BrowserHelper.SwitchToLastOpenedWindow();
        }

        public List<string> GetTableContent()
        {
            var elements = GetFirstColumnOfTableFromXPath(QuestionnaireTableRowsPath, QuestionnaireTableId);
            return elements;
        }

        public void ClickReturnToSurveyList()
        {
            ClickButtonById(ReturnSurveyId);
        }
    }
}
