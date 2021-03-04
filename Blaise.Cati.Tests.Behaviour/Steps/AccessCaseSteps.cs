using System;
using System.Diagnostics;
using System.Threading;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
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
            CatiInterviewHelper.GetInstance().CreateInterviewUser();
            InstrumentHelper.GetInstance().InstallInstrument();
        }

        [Given(@"There is a questionnaire installed on a Blaise environment")]
        public void GivenThereIsAQuestionnaireInstalledOnABlaiseEnvironment()
        {
        }


        [Given(@"I log on to Cati as an interviewer")]
        public void GivenILogOnToCatiAsAnInterviewer()
        {
            CatiManagementHelper.GetInstance().LogIntoCatiManagementPortalAsAnInterviewer();
        }


        [When(@"I click the play button for case '(.*)'")]
        public void WhenIClickThePlayButtonForCase(string caseId)
        {
            try
            {
                CatiInterviewHelper.GetInstance().ClickPlayButtonToAccessCase();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "LogOnInterview", "Log onto Interview Screen");
            }
        }

        [When(@"The the time is within the day batch parameters")]
        public void WhenTheTheTimeIsWithinTheDayBatchParameters()
        {
            try
            {
                CatiInterviewHelper.GetInstance().AddSurveyFilter();
                CatiInterviewHelper.GetInstance().SetupDayBatchTimeParameters();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "SetDayBatchParameters", "Set Daybatch parameters page");
            }
        }

        [When(@"I Open the cati scheduler as an interviewer")]
        public void WhenIOpenTheCatiSchedulerAsAnInterviewer()
        {
            CatiInterviewHelper.GetInstance().AccessInterviewPortal();
        }

        [Then(@"I am able to capture the respondents data for case '(.*)'")]
        public void ThenIAmAbleToCaptureTheRespondentsDataForCase(string caseId)
        {
            try
            {
                BrowserHelper.SwitchToLastOpenedWindow();
                CatiInterviewHelper.GetInstance().WaitForFirstFocusObject();
                Assert.True(BrowserHelper.CurrentWindowHTML().Contains(caseId));
            }
            catch (Exception e)
            {
                TestContext.WriteLine("Error from Test Context " + BrowserHelper.CurrentWindowHTML());
                TestContext.Progress.WriteLine("Error from Test Context progress " + BrowserHelper.CurrentWindowHTML());
                Debug.WriteLine("Error from debug: " + BrowserHelper.CurrentWindowHTML());
                Console.WriteLine("Error from console: " + BrowserHelper.CurrentWindowHTML());
                FailWithScreenShot(e, "CaptureData", "Capture respondents data");
            }
        }

        [AfterScenario("interview")]
        public void CleanUpScenario()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            BrowserHelper.CloseBrowser();         
        }

        [AfterFeature("interview")]
        public static void CleanUpFeature()
        {
            CatiInterviewHelper.GetInstance().DeleteInterviewUser();
            CaseHelper.GetInstance().DeleteCases();
            InstrumentHelper.GetInstance().UninstallSurvey();
        }

        private static void FailWithScreenShot(Exception e, string screenShotName, string screenShotDescription)
        {
            var screenShotFile = BrowserHelper.TakeScreenShot(TestContext.CurrentContext.WorkDirectory,
                screenShotName);

            TestContext.AddTestAttachment(screenShotFile, screenShotDescription);
            Assert.Fail($"The test failed to complete - {e.Message}");
        }
    }
}
