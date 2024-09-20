using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Questionnaire;
using Blaise.Tests.Helpers.Tobi;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Blaise.Tobi.Tests.Behaviour.Steps
{
    [Binding]
    public class TobiSteps
    {
        [BeforeFeature("tobi")]
        public static void BeforeFeature()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, BlaiseConfigurationHelper.QuestionnairePath, BlaiseConfigurationHelper.QuestionnaireInstallOptions);
            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "9001" } };
            CaseHelper.GetInstance().CreateCase(new CaseModel("9001", "110", "07000000000"));
            DayBatchHelper.GetInstance().SetSurveyDay(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
            DayBatchHelper.GetInstance().CreateDayBatch(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
        }

        [Given(@"there are live surveys")]
        [Given(@"I can view a list of live surveys")]
        public void GivenThereAreLiveSurveys()
        {
        }

        [When(@"I launch TOBI")]
        public void WhenILaunchTobi()
        {
            TobiHelper.GetInstance().LoadTobiHomePage();
        }

        [Given(@"I can view a list of questionnaires for a live survey")]
        public void GivenICanViewAListOfQuestionnairesForALiveSurvey()
        {
            TobiHelper.GetInstance().LoadQuestionnairePage();
        }

        [When(@"I select a survey")]
        public void WhenISelectASurvey()
        {
            TobiHelper.GetInstance().LoadTobiHomePage();
            var currentUrl = TobiHelper.GetInstance().ClickLoadQuestionnaire();

            Assert.That(currentUrl,
                Is.EqualTo(TobiConfigurationHelper.SurveyUrl),
                $"Current URL should match the expected survey URL: {TobiConfigurationHelper.SurveyUrl}");
        }

        [When(@"I select a questionnaire")]
        public void WhenISelectAQuestionnaire()
        {
            TobiHelper.GetInstance().ClickInterviewButton(BlaiseConfigurationHelper.QuestionnaireName);
        }

        [Then(@"I am presented with a list of live surveys")]
        public void ThenIAmPresentedWithAListOfLiveSurveys()
        {
            var surveyTableContents = TobiHelper.GetInstance().GetSurveyTableContents();

            Assert.That(surveyTableContents,
                Contains.Item("DST"),
                "List of live surveys should contain 'DST'");
        }

        [Then(@"I am presented with a list of questionnaires for the survey")]
        public void ThenIAmPresentedWithAListOfQuestionnairesForTheSurvey()
        {
            var activeQuestionnaires = TobiHelper.GetInstance().GetQuestionnaireTableContents();

            Assert.That(activeQuestionnaires,
                Has.Some.Contain(BlaiseConfigurationHelper.QuestionnaireName),
                $"List of active questionnaires should contain '{BlaiseConfigurationHelper.QuestionnaireName}'");
        }

        [Then(@"I am presented with the Blaise login")]
        public void ThenIAmPresentedWithTheBlaiseLogin()
        {
            CatiInterviewHelper.GetInstance().LoginButtonIsAvailable();

            Assert.That(BrowserHelper.CurrentUrl.ToLower(),
                Is.EqualTo($"{CatiConfigurationHelper.SchedulerUrl.ToLower()}/login"),
                $"Current URL should be the Blaise login page: {CatiConfigurationHelper.SchedulerUrl.ToLower()}/login");
        }

        [AfterFeature("tobi")]
        public static void AfterFeature()
        {
            DayBatchHelper.GetInstance().RemoveSurveyDays(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
            CaseHelper.GetInstance().DeleteCases();
            QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }
    }
}
