using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Instrument;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class AccessCaseSteps
    {
        [BeforeFeature("interview")]
        public static void InitializeFeature()
        {
            InstrumentHelper.GetInstance().InstallInstrument();

            CatiManagementHelper.GetInstance().CreateAdminUser();
            CatiInterviewHelper.GetInstance().CreateInterviewUser();
        }

        [Given(@"There is a questionnaire installed on a Blaise environment")]
        public void GivenThereIsAQuestionnaireInstalledOnABlaiseEnvironment()
        {
            Assert.IsTrue(InstrumentHelper.GetInstance()
                .SurveyHasInstalled(BlaiseConfigurationHelper.InstrumentName, 20));
        }


        [Given(@"I log on to Cati as an interviewer")]
        public void GivenILogOnToCatiAsAnInterviewer()
        {
            CatiManagementHelper.GetInstance().LogIntoCatiManagementPortalAsAnInterviewer();
        }


        [When(@"I click the play button for case '(.*)'")]
        public void WhenIClickThePlayButtonForCase(string caseId)
        {
            CatiInterviewHelper.GetInstance().ClickPlayButtonToAccessCase(caseId);
        }

        [When(@"The time is within the day batch parameters")]
        public void WhenTheTimeIsWithinTheDayBatchParameters()
        {
            CatiInterviewHelper.GetInstance().AddSurveyFilter();
            CatiInterviewHelper.GetInstance().SetupDayBatchTimeParameters();
        }

        [When(@"I Open the cati scheduler as an interviewer")]
        public void WhenIOpenTheCatiSchedulerAsAnInterviewer()
        {
            CatiInterviewHelper.GetInstance().AccessInterviewPortal();
        }

        [Then(@"I am able to capture the respondents data for case '(.*)'")]
        public void ThenIAmAbleToCaptureTheRespondentsDataForCase(string caseId)
        {
            BrowserHelper.SwitchToLastOpenedWindow();
            CatiInterviewHelper.GetInstance().WaitForFirstFocusObject();
            BrowserHelper.WaitForTextInHTML(caseId);
        }

        [AfterScenario("interview")]
        public void CleanUpScenario()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            BrowserHelper.ClosePreviousTab();
        }

        [AfterFeature("interview")]
        public static void CleanUpFeature()
        {
            CaseHelper.GetInstance().DeleteCases();
            InstrumentHelper.GetInstance().UninstallSurvey();
            CatiInterviewHelper.GetInstance().DeleteAdminUser();
            CatiInterviewHelper.GetInstance().DeleteInterviewUser();
            BrowserHelper.ClearSessionData();
        }
    }
}
