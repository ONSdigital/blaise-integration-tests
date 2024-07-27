﻿using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Questionnaire;
using NUnit.Framework;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class AccessCaseSteps
    {
        private readonly ScenarioContext _scenarioContext;
        public AccessCaseSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }


        [BeforeFeature("interview")]
        public static void InitializeFeature()
        {
            try
            {
                CatiInterviewHelper.GetInstance().CreateInterviewUser();
                QuestionnaireHelper.GetInstance().InstallQuestionnaire();
                Assert.IsTrue(QuestionnaireHelper.GetInstance()
                    .SurveyHasInstalled(BlaiseConfigurationHelper.QuestionnaireName, 60));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error from debug: {ex.Message}, inner exception: {{ex.InnerException?.Message}}\"");
                Console.WriteLine($"Error from console: {ex.Message}, inner exception: {{ex.InnerException?.Message}}\"");
                Assert.Fail($"The test failed to complete - {ex.Message}, inner exception: {ex.InnerException?.Message}");
            }
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
            try
            {
                BrowserHelper.SwitchToLastOpenedWindow();
                CatiInterviewHelper.GetInstance().WaitForFirstFocusObject();
                BrowserHelper.WaitForTextInHTML(caseId);
            }
            catch
            {
                TestContext.WriteLine("Error from Test Context " + BrowserHelper.CurrentWindowHTML());
                TestContext.Progress.WriteLine("Error from Test Context progress " + BrowserHelper.CurrentWindowHTML());
                Debug.WriteLine("Error from debug: " + BrowserHelper.CurrentWindowHTML());
                Console.WriteLine("Error from console: " + BrowserHelper.CurrentWindowHTML());
                throw;
            }
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
            CatiInterviewHelper.GetInstance().DeleteInterviewUser();
            QuestionnaireHelper.GetInstance().UninstallSurvey();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            BrowserHelper.ClearSessionData();
        }
    }
}
