namespace Blaise.Tests.Helpers.Tobi.Pages
{
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;

    public class QuestionnairePage : BasePage
    {
        private const string _questionnaireTableId = "instrument-table";
        private const string _questionnaireTableRowsPath = "//*[@id='instrument-table']/tbody/tr";
        private const string _returnSurveyId = "return-to-survey-list";

        public QuestionnairePage()
            : base(TobiConfigurationHelper.SurveyUrl)
        {
        }

        public void ClickInterviewButton(string questionnaire)
        {
            var questionnaireIndex = GetTableContent().FindIndex(s => s.Contains(questionnaire)) + 1;
            var interviewLinkPath = $"{_questionnaireTableRowsPath}[{questionnaireIndex}]/td[3]/a";
            ClickButtonByXPath(interviewLinkPath);
            BrowserHelper.SwitchToLastOpenedWindow();
        }

        public List<string> GetTableContent()
        {
            var elements = GetFirstColumnOfTableFromXPath(_questionnaireTableRowsPath, _questionnaireTableId);
            return elements;
        }

        public void ClickReturnToSurveyList()
        {
            ClickButtonById(_returnSurveyId);
        }
    }
}
