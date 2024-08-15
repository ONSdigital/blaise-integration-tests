using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Questionnaire;
using Blaise.Tests.Helpers.Tobi;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using System;
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
            QuestionnaireHelper.GetInstance().InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, BlaiseConfigurationHelper.QuestionnairePath);
            CaseHelper.GetInstance().CreateCase(new CaseModel("9001", "110", "07000000000"));
            DayBatchHelper.GetInstance().SetSurveyDay(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
            DayBatchHelper.GetInstance().CreateDayBatch(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
        }

        [Given(@"there are live surveys")]
        [Given(@"I can view a list of live surveys")]
        public void GivenThereAreLiveSurveys()
        {
        }

        [Given(@"I can view a list of surveys on Blaise within TOBI")]
        [Given(@"a survey questionnaire end date has passed")]
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

        [Given(@"I have selected a survey")]
        [When(@"I select a survey")]
        public void WhenISelectASurvey()
        {
            TobiHelper.GetInstance().LoadTobiHomePage();
            var currentUrl = TobiHelper.GetInstance().ClickLoadQuestionnaire();
            Assert.AreEqual(TobiConfigurationHelper.SurveyUrl, currentUrl);
        }

        [When(@"I select a questionnaire")]
        public void WhenISelectAQuestionnaire()
        {
            TobiHelper.GetInstance().ClickInterviewButton(BlaiseConfigurationHelper.QuestionnaireName);
        }

        [When(@"I do not see the questionnaire that I am working on")]
        public void WhenIDoNotSeeTheQuestionnaireThatIAmWorkingOn()
        {
            Assert.AreEqual(TobiConfigurationHelper.SurveyUrl, BrowserHelper.CurrentUrl);
        }

        [Then(@"I am presented with a list of live surveys")]
        public void ThenIAmPresentedWithAListOfLiveSurveys()
        {
            Assert.IsNotNull(TobiHelper.GetInstance().GetSurveyTableContents().Where(s => s.Equals("DST")));   
        }

        [Then(@"I am presented with a list of questionnaires for the survey")]
        public void ThenIAmPresentedWithAListOfQuestionnairesForTheSurvey()
        {
            var activeQuestionnaires = TobiHelper.GetInstance().GetQuestionnaireTableContents();
            Assert.IsNotNull(activeQuestionnaires.Where(q => q.Contains(BlaiseConfigurationHelper.QuestionnaireName)));
        }

        [Then(@"I am presented with the Blaise login")]
        public void ThenIAmPresentedWithTheBlaiseLogin()
        {
            CatiInterviewHelper.GetInstance().LoginButtonIsAvailable();
            Assert.AreEqual($"{CatiConfigurationHelper.SchedulerUrl.ToLower()}/login", BrowserHelper.CurrentUrl.ToLower());   
        }

        [Then(@"I will not see that questionnaire listed for the survey")]
        public void ThenIWillNotSeeThatQuestionnaireListedForTheSurvey()
        {
            var questionnaireShowing = TobiHelper.GetInstance().GetQuestionnaireTableContents()
                .Where(s => s.Contains(BlaiseConfigurationHelper.QuestionnaireName));
            Assert.IsEmpty(questionnaireShowing);
        }

        [Then(@"I will not see any surveys listed")]
        public void ThenIWillNotSeeAnySurveysListed()
        {
            Assert.AreEqual("No active surveys found.", TobiHelper.GetInstance().GetNoSurveysText());
        }

        [Then(@"I am able to go back to view the list of surveys")]
        public void ThenIAmAbleToGoBackToViewTheListOfSurveys()
        {
            TobiHelper.GetInstance().ClickReturnToSurveyListButton();
            Assert.AreEqual(TobiConfigurationHelper.TobiUrl, BrowserHelper.CurrentUrl);
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
