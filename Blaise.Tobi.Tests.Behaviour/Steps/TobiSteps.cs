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
            CaseHelper.GetInstance().CreateCase(new CaseModel("900001", "110", "07000 000 00"));
            DayBatchHelper.GetInstance().SetSurveyDay(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
            DayBatchHelper.GetInstance().CreateDayBatch(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
        }

        [Given(@"I have internet access")]
        public void GivenIHaveInternetAccess()
        {
        }

        [Given(@"I can view a list of surveys on Blaise within TOBI")]
        [Given(@"a survey questionnaire end date has passed")]
        [When(@"I launch TOBI")]
        public void WhenILaunchTobi()
        {
            TobiHelper.GetInstance().LoadTobiHomePage();
        }

        [Given(@"I can view a list of live questionnaires for the survey I am allocated to")]
        public void GivenICanViewAListOfLiveQuestionnairesForTheSurveyIAmAllocatedTo()
        {
            TobiHelper.GetInstance().LoadQuestionnairePagePage();
        }

        [Given(@"I have selected a survey")]
        [When(@"I select the DST survey I am working on")]
        public void WhenISelectTheDSTSurveyIAmWorkingOn()
        {
            TobiHelper.GetInstance().LoadTobiHomePage();
            var currentUrl = TobiHelper.GetInstance().ClickLoadQuestionnaire();
            Assert.AreEqual(TobiConfigurationHelper.SurveyUrl, currentUrl);
        }

        [When(@"I select a link to interview against the questionnaire with the survey dates I am working on")]
        public void WhenISelectALinkToInterviewAgainstTheQuestionnaireWithTheSurveyDatesIAmWorkingOn()
        {
            TobiHelper.GetInstance().ClickInterviewButton(BlaiseConfigurationHelper.QuestionnaireName);
        }

        [When(@"I do not see the questionnaire that I am working on")]
        public void WhenIDoNotSeeTheQuestionnaireThatIAmWorkingOn()
        {
            Assert.AreEqual(TobiConfigurationHelper.SurveyUrl, BrowserHelper.CurrentUrl);
        }

        [Then(@"I will be able to view all live surveys with questionnaires loaded in Blaise, identified by their three letter acronym \(TLA\), i\.e\. OPN, LMS")]
        public void ThenIWillBeAbleToViewAllLiveSurveysWithQuestionnairesLoadedInBlaiseIdentifiedByTheirThreeLetterAcronym()
        {
            Assert.IsNotNull(TobiHelper.GetInstance().GetSurveyTableContents().Where(s => s.Equals("DST")));   
        }

        [Then(@"I am presented with a list of active questionnaires to be worked on that day for that survey")]
        public void ThenIAmPresentedWithAListOfActiveQuestionnairesToBeWorkedOnThatDayForThatSurvey()
        {
            var activeQuestionnaires = TobiHelper.GetInstance().GetQuestionnaireTableContents();
            Assert.IsNotNull(activeQuestionnaires.Where(q => q.Contains(BlaiseConfigurationHelper.QuestionnaireName)));
            
        }

        [Then(@"I am presented with the Blaise log in")]
        public void ThenIAmPresentedWithTheBlaiseLogIn()
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
            BrowserHelper.ClosePreviousTab();
            CaseHelper.GetInstance().DeleteCases();
            QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            BrowserHelper.ClearSessionData();
        }

    }
}
