namespace Blaise.Cati.Tests.Behaviour.Steps
{
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Helpers.Cati.Pages;
    using OpenQA.Selenium;
    using Reqnroll;
    using System;

    [Binding]
    public sealed class AccessCasesSteps
    {
        [When(@"I click the play button for case '(.*)'")]
        public void WhenIClickThePlayButtonForCase(string caseId)
        {
            CatiInterviewHelper.GetInstance().ClickPlayButtonToAccessCase(caseId);
        }

        [When(@"the time is within the daybatch parameters")]
        public void WhenTheTimeIsWithinTheDaybatchParameters()
        {
            var daybatchPage = new DaybatchPage();

            // Navigate to the Daybatch page
            daybatchPage.NavigateToVersionSpecificPage();

            if (daybatchPage.IsUsingNewSelectors)
            {
                // Wait for the Daybatch table to load for the new dashboard
                if (!BrowserHelper.ElementExistsByXPath("//*[@id='Daybatch_content_table']", TimeSpan.FromSeconds(30)))
                {
                    throw new Exception("Daybatch table did not load within the expected time.");
                }
            }

            // Apply survey filter and set daybatch time parameters
            CatiInterviewHelper.GetInstance().AddSurveyFilter();
            CatiInterviewHelper.GetInstance().SetupDaybatchTimeParameters();
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
