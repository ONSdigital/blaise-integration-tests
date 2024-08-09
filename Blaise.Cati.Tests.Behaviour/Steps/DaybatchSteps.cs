using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Questionnaire;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DaybatchSteps
    {
        private static ScenarioContext _scenarioContext;
        public DaybatchSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeFeature("daybatch")]
        public static void InitializeFeature()
        {
            try
            {
                CatiManagementHelper.GetInstance().CreateAdminUser();
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

        [Given(@"I log on to Cati as an administrator")]
        public void GivenILogOnToTheCatiDashboard()
        {
            CatiManagementHelper.GetInstance().LogIntoCatiManagementPortal();
            Assert.AreNotEqual(CatiConfigurationHelper.LoginUrl, CatiManagementHelper.GetInstance().CurrentUrl(),
                "Expected to leave the login page");
        }

        [Given(@"I have created a daybatch for today")]
        [When(@"I create a daybatch for today")]
        public void WhenICreateADaybatchForToday()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            CatiManagementHelper.GetInstance().CreateDayBatch();
        }
        [When(@"the sample cases are present on the daybatch entry screen")]
        [Then(@"the sample cases are present on the daybatch entry screen")]
        public void ThenTheSampleCasesArePresentOnTheDaybatchEntryScreen(IEnumerable<CaseModel> cases)
        {
            var entriesText = CatiManagementHelper.GetInstance().GetDaybatchEntriesText();
            Assert.IsNotNull(entriesText);
        }

        [AfterScenario("daybatch")]
        public void CleanUpFeature()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            CatiManagementHelper.GetInstance().DeleteAdminUser();
            CaseHelper.GetInstance().DeleteCases();
            QuestionnaireHelper.GetInstance().UninstallQuestionnaire();
            BrowserHelper.ClearSessionData();
        }
    }
}
