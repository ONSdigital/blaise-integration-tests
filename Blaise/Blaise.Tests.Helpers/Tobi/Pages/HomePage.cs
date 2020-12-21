using System.Collections.Generic;
using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Tobi.Pages
{
    public class HomePage : BasePage
    {
        public string LaunchQuestionnaireLinkPath = "//*[@id='survey-table']/tbody/tr/td[2]/a";
        public string SurveyTablePath = "//*[@id='survey-table']/tbody/tr";
        public string SurveyTableId = "survey-table";

        public HomePage() : base(TobiConfigurationHelper.TobiUrl)
        {
        }

        public void ClickQuestionnaireButton()
        {
            ClickButtonByXPath(LaunchQuestionnaireLinkPath);
        }

        public List<string> GetSurveyAcronyms()
        {
            var elements = GetFirstColumnOfTableFromXPath(SurveyTablePath, SurveyTableId);
            return elements;
        }
    }
}
