using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Questionnaire;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class CommonHooks
    {
        private readonly ScenarioContext _scenarioContext;

        private static bool _hasFailureOccurred = false;

        public CommonHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, BlaiseConfigurationHelper.QuestionnairePath);
            CatiManagementHelper.GetInstance().CreateAdminUser();
            CatiInterviewHelper.GetInstance().CreateInterviewUser();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            CatiInterviewHelper.GetInstance().DeleteInterviewUser();
            CatiManagementHelper.GetInstance().DeleteAdminUser();
            QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            if (_hasFailureOccurred)
            {
                Assert.Fail("A previous scenario has failed. Skipping test.");
            }
        }

        [AfterStep]
        public void AfterStep()
        {
            if (_scenarioContext.TestError != null)
            {
                _hasFailureOccurred = true;
                BrowserHelper.OnError(TestContext.CurrentContext, _scenarioContext);
                throw new Exception(_scenarioContext.TestError.Message);
            }
        }
    }
}
