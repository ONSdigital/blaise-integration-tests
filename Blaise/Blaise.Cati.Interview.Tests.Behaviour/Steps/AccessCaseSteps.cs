using System;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Instrument;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Interview.Tests.Behaviour.Steps
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
                FailWithScreenShot(e);
            }
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
                FailWithScreenShot(e);
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

        private static void FailWithScreenShot(Exception e)
        {
            var screenShotFile = BrowserHelper.TakeScreenShot(TestContext.CurrentContext.WorkDirectory,
                "CatiInterview.png");

            TestContext.AddTestAttachment(screenShotFile, "Cati Interview Screen");
            Assert.Fail($"The test failed to complete - {e.Message}");
        }
    }
}
