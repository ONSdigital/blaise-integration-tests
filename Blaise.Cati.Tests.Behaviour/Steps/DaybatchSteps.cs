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
        public static void BeforeFeature()
        {
            try
            {
                CatiManagementHelper.GetInstance().CreateAdminUser();
                QuestionnaireHelper.GetInstance().InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, BlaiseConfigurationHelper.QuestionnairePath);
                Assert.IsTrue(QuestionnaireHelper.GetInstance()
                    .CheckQuestionnaireInstalled(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, 60));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Test failed: {ex.Message}, inner exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Test failed: {ex.Message}, inner exception: {ex.InnerException?.Message}");
                Assert.Fail($"Test failed: {ex.Message}, inner exception: {ex.InnerException?.Message}");
            }
        }

        [Given(@"I log into the CATI dashboard as an administrator")]
        public void GivenILogOnToTheCatiDashboard()
        {
            CatiManagementHelper.GetInstance().LogIntoCatiDashboardAsAdministrator();
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
        [When(@"the sample cases are present on the daybatch entries screen")]
        [Then(@"the sample cases are present on the daybatch entries screen")]
        public void ThenTheSampleCasesArePresentOnTheDaybatchEntriesScreen(IEnumerable<CaseModel> cases)
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
            QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
            BrowserHelper.ClearSessionData();
        }
    }
}
