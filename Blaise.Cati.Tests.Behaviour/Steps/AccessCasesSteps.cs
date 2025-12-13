namespace Blaise.Cati.Tests.Behaviour.Steps
{
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Cati;
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
    }
}
