using System.Collections.Generic;
using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Tobi.Pages
{
    public class QuestionnairePage : BasePage
    {
        public string InterviewLinkPath = "//*[@id='instrument-table']/tbody/tr/td[3]/a";
        public string QuestionnaireTableId = "instrument-table";
        public string QuestionnaireTableRowsPath = "//*[@id='instrument-table']/tbody/tr";
        public string ReturnSurveyPath = "//*[@id='main-content']/p[4]/a";
        
        //*[@id="instrument-table"]/tbody/tr[1]/td[1]
        //*[@id="instrument-table"]/tbody/tr[2]/td[1]
        public QuestionnairePage() : base(TobiConfigurationHelper.SurveyUrl)
        {
        }

        public void ClickInterviewButton()
        {
            ClickButtonByXPath(InterviewLinkPath);
        }

        public List<string> GetTableContent()
        {
            var elements = GetFirstColumnOfTableFromXPath(QuestionnaireTableRowsPath, QuestionnaireTableId);
            return elements;
        }

        public void ClickReturnToSurveyList()
        {
            ClickButtonByXPath(ReturnSurveyPath);
        }
    }
}
