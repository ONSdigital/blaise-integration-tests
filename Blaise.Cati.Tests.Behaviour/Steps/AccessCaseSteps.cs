using System;
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
        }

        [When(@"I log on to the Interviewing Portal as an interviewer")]
        public void WhenIAccessTheCaseFromTheCaseInterviewingScreen()
        {
            try
            {
                CatiInterviewHelper.GetInstance().LogIntoInterviewPortal();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "LogOnInterview", "Log onto Interview Screen");
            }
        }

        [When(@"The the time is within the day batch parameters")]
        public void WhenTheTheTimeIsWithinTheDayBatchParameters()
        {
            CatiInterviewHelper.GetInstance().SetupDayBatchTimeParameters();
        }


        [Then(@"I am able to capture the respondents data for case '(.*)'")]
        public void ThenIAmAbleToCaptureTheRespondentsDataForCase(string caseId)
        {
            try
            {
                var caseIdText = CatiInterviewHelper.GetInstance().GetCaseIdText();
                Assert.AreEqual($"Case: {caseId}", caseIdText);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "CaptureData", "Capture respondents data");
            }
        }

        [AfterFeature("interview")]
        public static void CleanUpFeature()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            BrowserHelper.CloseBrowser();
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
