namespace Blaise.Tobi.Tests.Behaviour.Steps
{
    using System;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Health;
    using Blaise.Tests.Helpers.Questionnaire;
    using NUnit.Framework;
    using Reqnroll;

    [Binding]
    public sealed class CommonSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            HealthCheckHelper.CheckBlaiseConnection();
            HealthCheckHelper.CheckUrlIsAvailable(CatiConfigurationHelper.CatiBaseUrl);
            HealthCheckHelper.CheckUrlIsAvailable(DqsConfigurationHelper.DqsUrl);
            HealthCheckHelper.CheckUrlIsAvailable(TobiConfigurationHelper.TobiUrl);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            QuestionnaireHelper.GetInstance()
                .EnsureQuestionnaireReadyForTest(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }

        [AfterStep]
        public void AfterStep()
        {
            if (_scenarioContext.TestError != null)
            {
                BrowserHelper.OnError(TestContext.CurrentContext, _scenarioContext);
                throw new Exception(_scenarioContext.TestError.Message);
            }
        }
    }
}
