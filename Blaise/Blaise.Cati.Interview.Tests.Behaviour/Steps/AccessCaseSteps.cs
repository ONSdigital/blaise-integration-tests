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
            CatiInterviewHelper.GetInstance().LogIntoInterviewPortal();
        }

        [Then(@"I am able to capture the respondents data for case '(.*)'")]
        public void ThenIAmAbleToCaptureTheRespondentsDataForCase(string caseId)
        {
            var caseIdText = CatiInterviewHelper.GetInstance().GetCaseIdText();
            Assert.AreEqual($"Case: {caseId}", caseIdText);
        }

        [AfterFeature("interview")]
        public static void CleanUpFeature()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEnteries();
            BrowserHelper.Dispose();
            CatiInterviewHelper.GetInstance().DeleteInterviewUser();
            CaseHelper.GetInstance().DeleteCases();
            InstrumentHelper.GetInstance().UninstallSurvey();
        }
    }
}
