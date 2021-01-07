using System;
using System.Linq;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Instrument;
using Blaise.Tests.Helpers.Tobi;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using StatNeth.Blaise.Runtime.Cati.BusinessLogic.ManagementBlocks;
using TechTalk.SpecFlow;

namespace Blaise.Tobi.Tests.Behaviour.Steps
{
    [Binding]
    public class TobiSteps
    {
        [BeforeFeature("tobi")]
        public static void InitializeFeature()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
            CaseHelper.GetInstance().CreateCase(new CaseModel("900000", "110", "07000 000 00"));
        }

        [BeforeScenario("HappyPath")]
        public static void HappyPathScenarios()
        {
            DayBatchHelper.GetInstance().SetSurveyDay(BlaiseConfigurationHelper.InstrumentName ,DateTime.Today);
            DayBatchHelper.GetInstance().CreateDayBatch(BlaiseConfigurationHelper.InstrumentName, DateTime.Today);
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

        [Given(@"Another survey is active")]
        public void GivenAnotherSurveyIsActive()
        {
            InstrumentHelper.GetInstance().InstallInstrument(BlaiseConfigurationHelper.SecondInstrumentPackage);
            DayBatchHelper.GetInstance().SetSurveyDay(BlaiseConfigurationHelper.SecondInstrumentName, DateTime.Today);
        }

        [Given(@"I have selected a survey")]
        [When(@"I select the survey I am working on")]
        public void WhenISelectTheSurveyIAmWorkingOn()
        {
            TobiHelper.GetInstance().LoadTobiHomePage();
            var currentUrl = TobiHelper.GetInstance().ClickLoadQuestionnaire();
            Assert.AreEqual(TobiConfigurationHelper.SurveyUrl, currentUrl);
        }

        [When(@"I select a link to interview against the questionnaire with the survey dates I am working on")]
        public void WhenISelectALinkToInterviewAgainstTheQuestionnaireWithTheSurveyDatesIAmWorkingOn()
        {
            TobiHelper.GetInstance().ClickInterviewButton();
        }

        [When(@"I do not see the questionnaire that I am working on")]
        public void WhenIDoNotSeeTheQuestionnaireThatIAmWorkingOn()
        {
            Assert.AreEqual(TobiConfigurationHelper.SurveyUrl, BrowserHelper.CurrentUrl);
        }

        [Then(@"I will be able to view all live surveys with questionnaires loaded in Blaise, identified by their three letter acronym \(TLA\), i\.e\. OPN, LMS")]
        public void ThenIWillBeAbleToViewAllLiveSurveysWithQuestionnairesLoadedInBlaiseIdentifiedByTheirThreeLetterAcronym()
        {
            Assert.AreEqual("OPN", TobiHelper.GetInstance().GetSurveyTableContents().FirstOrDefault());
        }

        [Then(@"I am presented with a list of active questionnaires to be worked on that day for that survey")]
        public void ThenIAmPresentedWithAListOfActiveQuestionnairesToBeWorkedOnThatDayForThatSurvey()
        {
            Assert.AreEqual(BlaiseConfigurationHelper.InstrumentName, TobiHelper.GetInstance().GetFirstQuestionnaireInTable());
        }

        [Then(@"I am presented with the Blaise log in")]
        public void ThenIAmPresentedWithTheBlaiseLogIn()
        {
            CatiInterviewHelper.GetInstance().LoginButtonIsAvailable();
            Assert.AreEqual($"{CatiConfigurationHelper.InterviewUrl.ToLower()}login", BrowserHelper.CurrentUrl.ToLower());
        }

        [Then(@"I will not see that questionnaire listed for the survey")]
        public void ThenIWillNotSeeThatQuestionnaireListedForTheSurvey()
        {
            var questionnaireShowing = TobiHelper.GetInstance().GetQuestionnaireTableContents()
                .Where(s => s.Contains(BlaiseConfigurationHelper.InstrumentName));
            Assert.IsEmpty(questionnaireShowing);
        }

        [Then(@"I will not see any surveys listed")]
        public void ThenIWillNotSeeAnySurveysListed()
        {
            Assert.AreEqual("No active surveys found." ,TobiHelper.GetInstance().GetNoSurveysText());
        }

        [Then(@"I am able to go back to view the list of surveys")]
        public void ThenIAmAbleToGoBackToViewTheListOfSurveys()
        {
            TobiHelper.GetInstance().ClickReturnToSurveyListButton();
            Assert.AreEqual(TobiConfigurationHelper.TobiUrl, BrowserHelper.CurrentUrl);
        }

        [AfterScenario("SecondSurvey")]
        public static void CleanUpScenario()
        {
            InstrumentHelper.GetInstance().UninstallSurvey(BlaiseConfigurationHelper.SecondInstrumentName, BlaiseConfigurationHelper.ServerParkName);
        }

        [AfterScenario("HappyPath")]
        public static void CleanUpHappyPath()
        {
            CatiManagementHelper.GetInstance().LogIntoCatiManagementPortal();
            CatiManagementHelper.GetInstance().SetSurveyDays();
        }

        [AfterFeature("tobi1")]
        public static void CleanUpFeature()
        {
            CatiManagementHelper.GetInstance().LogIntoCatiManagementPortal();
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            BrowserHelper.CloseBrowser();
            CaseHelper.GetInstance().DeleteCases();
            InstrumentHelper.GetInstance().UninstallSurvey();
        }
    }
}
