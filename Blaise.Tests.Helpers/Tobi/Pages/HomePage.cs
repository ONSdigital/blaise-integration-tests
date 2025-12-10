namespace Blaise.Tests.Helpers.Tobi.Pages
{
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;

    public class HomePage : BasePage
    {
        private const string _surveyTablePath = "//*[@id='survey-table']/tbody/tr";
        private const string _surveyTableId = "survey-table";
        private const string _noResultsPath = "//p[contains(text(), 'No active surveys found.')]";

        public HomePage()
            : base(TobiConfigurationHelper.TobiUrl)
        {
        }

        public void ClickQuestionnaireButton()
        {
            var dstIndex = GetSurveyAcronyms().FindIndex(s => s.Contains("DST")) + 1;
            var launchQuestionnaireLinkPath = $"{_surveyTablePath}[{dstIndex}]/td[2]/a";
            ClickButtonByXPath(launchQuestionnaireLinkPath);
        }

        public List<string> GetSurveyAcronyms()
        {
            var elements = GetFirstColumnOfTableFromXPath(_surveyTablePath, _surveyTableId);
            return elements;
        }

        public string GetNoSurveysText()
        {
            return GetElementTextByPath(_noResultsPath);
        }
    }
}
