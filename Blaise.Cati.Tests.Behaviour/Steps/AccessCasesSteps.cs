using System;
using System.Diagnostics;
using System.Threading;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Cati;
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
    public void WhenTheTimeIsWithinTheDaybatchParameters()
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
        try
        {
            BrowserHelper.SwitchToLastOpenedWindow();
            var instance = CatiInterviewHelper.GetInstance();
            instance.WaitForFirstFocusObject();
            var currentUrl = BrowserHelper.CurrentUrl;
            var html = BrowserHelper.CurrentWindowHtml();

            // TODO: make this better... this is for the access case via scheduler page and there isn't anything better on the page to verify against
            if (caseId == "9002")
            {
                BrowserHelper.PopulateInputByName("wa_1haa", caseId);
                instance.ClickSubmitButton(caseId);
                BrowserHelper.WaitForTextInHtml("Welcome to the study");
            }
            else
            {
                BrowserHelper.WaitForTextInHtml(caseId);
            }
        }
        catch
        {
            TestContext.WriteLine("Error from Test Context " + BrowserHelper.CurrentWindowHtml());
            TestContext.Progress.WriteLine("Error from Test Context progress " + BrowserHelper.CurrentWindowHtml());
            Debug.WriteLine("Error from debug: " + BrowserHelper.CurrentWindowHtml());
            Console.WriteLine("Error from console: " + BrowserHelper.CurrentWindowHtml());
            throw;
        }
    }
}
