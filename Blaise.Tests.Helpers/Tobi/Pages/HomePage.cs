namespace Blaise.Tests.Helpers.Tobi.Pages
{
    using System;
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class HomePage : BasePage
    {
        private const string SurveyTablePath = "//*[@id='survey-table']/tbody/tr";
        private const string SurveyTableId = "survey-table";
        private const string NoResultsPath = "//p[contains(text(), 'No active surveys found.')]";

        public HomePage()
            : base(TobiConfigurationHelper.TobiUrl)
        {
        }

        protected override By PageIdentityBy =>
            By.XPath($"//*[@id='{SurveyTableId}'] | {NoResultsPath}");

        public void ClickQuestionnaireButton()
        {
            var questionnaireName = BlaiseConfigurationHelper.QuestionnaireName;
            var questionnairePrefix = questionnaireName.Length >= 3
                ? questionnaireName.Substring(0, 3)
                : questionnaireName;
            var surveyAcronyms = GetSurveyAcronyms();
            var questionnaireIndex = surveyAcronyms.FindIndex(s => s.Contains(questionnairePrefix));
            if (questionnaireIndex < 0)
            {
                throw new Exception($"Survey grouping '{questionnairePrefix}' for questionnaire '{questionnaireName}' not found in survey list. " +
                    $"Available: {string.Join(", ", surveyAcronyms)}");
            }

            var launchQuestionnaireLinkPath = $"{SurveyTablePath}[{questionnaireIndex + 1}]/td[2]/a";
            ClickButtonByXPath(launchQuestionnaireLinkPath);
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
