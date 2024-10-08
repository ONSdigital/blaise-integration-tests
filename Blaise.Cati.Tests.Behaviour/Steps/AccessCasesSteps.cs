﻿using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Cati;
using NUnit.Framework;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class AccessCasesSteps
    {
        [Given(@"there is a CATI questionnaire installed")]
        public void GivenThereIsACatiQuestionnaireInstalled()
        {
        }

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
            try
            {
                BrowserHelper.SwitchToLastOpenedWindow();
                CatiInterviewHelper.GetInstance().WaitForFirstFocusObject();
                BrowserHelper.WaitForTextInHtml(caseId);
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
}
