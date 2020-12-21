using System.Collections.Generic;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Tobi.Pages
{
    public class QuestionnairePage : BasePage
    {
        public string InterviewLinkPath = "//*[@id='instrument-table']/tbody/tr/td[3]/a";
        public string QuestionnaireTableId = "instrument-table";
        public string QuestionnaireTableRowsPath = "//*[@id='instrument-table']/tbody/tr";
        public string ReturnSurveyId = "return-to-survey-list";

        public QuestionnairePage() : base(TobiConfigurationHelper.SurveyUrl)
        {
        }

        public void ClickInterviewButton()
        {
            ClickButtonByXPath(InterviewLinkPath);
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
