namespace Blaise.Tobi.Tests.Behaviour.Steps
{
    using System;
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Tobi;
    using NUnit.Framework;
    using Reqnroll;

    [Binding]
    public class TobiSteps
    {
        [Given(@"there are live surveys")]
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
            GivenThereAreLiveSurveys();
            TobiHelper.GetInstance().LoadQuestionnairePage();
        }

        [When(@"I select a survey")]
        public void WhenISelectASurvey()
        {
            TobiHelper.GetInstance().LoadTobiHomePage();
            var currentUrl = TobiHelper.GetInstance().ClickLoadQuestionnaire();

            Assert.That(
                currentUrl,
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

            Assert.That(
                surveyTableContents,
                Contains.Item("DST"),
                "List of live surveys should contain DST");
        }

        [Then(@"I am presented with a list of questionnaires for the survey")]
        public void ThenIAmPresentedWithAListOfQuestionnairesForTheSurvey()
        {
            var activeQuestionnaires = TobiHelper.GetInstance().GetQuestionnaireTableContents();

            Assert.That(
                activeQuestionnaires,
                Has.Some.Contain(BlaiseConfigurationHelper.QuestionnaireName),
                $"List of active questionnaires should contain {BlaiseConfigurationHelper.QuestionnaireName}");
        }

        [Then(@"I am presented with the Blaise login")]
        public void ThenIAmPresentedWithTheBlaiseLogin()
        {
            CatiInterviewHelper.GetInstance().LoginButtonIsAvailable();

            Assert.That(
                BrowserHelper.CurrentUrl.ToLower(),
                Is.EqualTo($"{CatiConfigurationHelper.SchedulerUrl.ToLower()}/login"),
                $"Current URL should be the Blaise login page: {CatiConfigurationHelper.SchedulerUrl.ToLower()}/login");
        }
    }
}
