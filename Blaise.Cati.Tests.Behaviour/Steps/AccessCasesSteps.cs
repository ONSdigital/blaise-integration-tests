namespace Blaise.Cati.Tests.Behaviour.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Case;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Models.Case;
    using NUnit.Framework;
    using Reqnroll;

    [Binding]
    public sealed class AccessCasesSteps
    {
        [When(@"I click the play button for case '(.*)'")]
        public void WhenIClickThePlayButtonForCase(string caseId)
        {
            CatiInterviewHelper.GetInstance().ClickPlayButtonToAccessCase(caseId);
        }

        [When(@"the time is within the daybatch parameters")]
        public void WhenTheTimeIsWithinTheDayBatchParameters()
        {
            CatiInterviewHelper.GetInstance().AddSurveyFilter();
            CatiInterviewHelper.GetInstance().SetupDayBatchTimeParameters();
        }

        [When(@"I open the CATI scheduler as an interviewer")]
        public void WhenIOpenTheCatiSchedulerAsAnInterviewer()
        {
            CatiInterviewHelper.GetInstance().AccessCatiScheduler();
        }

        [Then(@"I am able to capture the respondents data for case '(.*)'")]
        public void ThenIAmAbleToCaptureTheRespondentsDataForCase(string caseId)
        {
            BrowserHelper.SwitchToLastOpenedWindow();
            CatiInterviewHelper.GetInstance().WaitForFirstFocusObject();
            BrowserHelper.WaitForTextInHtml(caseId);
        }

        [Then(@"the cases are available in the questionnaire")]
        public void ThenTheCasesAreAvailableInTheQuestionnaire(IEnumerable<CaseModel> cases)
        {
            var expectedCases = cases.ToList();
            CheckNumberOfCasesMatch(expectedCases.Count);
            CheckCasesMatch(expectedCases);
        }

        private void CheckNumberOfCasesMatch(int expectedNumberOfCases)
        {
            var actualNumberOfCases = CaseHelper.GetInstance().NumberOfCasesInQuestionnaire();

            if (expectedNumberOfCases != actualNumberOfCases)
            {
                Assert.Fail($"Expected '{expectedNumberOfCases}' cases in Blaise, but {actualNumberOfCases} cases were found");
            }
        }

        private void CheckCasesMatch(IEnumerable<CaseModel> expectedCases)
        {
            var actualCases = CaseHelper.GetInstance().GetCasesInBlaise().ToList();

            foreach (var expectedCase in expectedCases)
            {
                var actualCase = actualCases.FirstOrDefault(c => c.PrimaryKey == expectedCase.PrimaryKey);

                Assert.That(actualCase, Is.Not.Null, $"Case '{expectedCase.PrimaryKey}' was not found in Blaise");

                Assert.That(actualCase, Is.EqualTo(expectedCase), $"Case '{expectedCase.PrimaryKey}' did not match the case in Blaise");
            }
        }
    }
}
