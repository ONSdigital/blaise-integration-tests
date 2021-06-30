using System.Collections.Generic;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Tobi.Pages
{
    public class HomePage : BasePage
    {
        public string SurveyTablePath = "//*[@id='survey-table']/tbody/tr";
        public string SurveyTableId = "survey-table";
        public string NoResultsPath = "//p[contains(text(), 'No active surveys found.')]";

        public HomePage() : base(TobiConfigurationHelper.TobiUrl)
        {
        }

        public void ClickQuestionnaireButton()
        {
            var dstIndex = GetSurveyAcronyms().FindIndex(s => s.Contains("DST")) + 1;
            var LaunchQuestionnaireLinkPath = $"{SurveyTablePath}[{dstIndex}]/td[2]/a";
            ClickButtonByXPath(LaunchQuestionnaireLinkPath);
        }

        public List<string> GetSurveyAcronyms()
        {
            var elements = GetFirstColumnOfTableFromXPath(SurveyTablePath, SurveyTableId);
            return elements;
        }

        public string GetNoSurveysText()
        {
            return GetElementTextByPath(NoResultsPath);
        }
    }
}
