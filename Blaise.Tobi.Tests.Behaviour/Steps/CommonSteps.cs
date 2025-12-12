namespace Blaise.Tobi.Tests.Behaviour.Steps
{
    using System;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework.Extensions;
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

            var catiUrl = ConfigurationExtensions.TryGetVariable("ENV_BLAISE_CATI_URL");
            if (!string.IsNullOrEmpty(catiUrl))
            {
                if (!catiUrl.StartsWith("http://") && !catiUrl.StartsWith("https://"))
                {
                    catiUrl = "https://" + catiUrl;
                }

                HealthCheckHelper.CheckUrlIsAvailable(catiUrl);
            }

            var dqsUrl = ConfigurationExtensions.TryGetVariable("ENV_DQS_URL");
            if (!string.IsNullOrEmpty(dqsUrl))
            {
                HealthCheckHelper.CheckUrlIsAvailable(dqsUrl);
            }

            var tobiUrl = ConfigurationExtensions.TryGetVariable("ENV_TOBI_URL");
            if (!string.IsNullOrEmpty(tobiUrl))
            {
                HealthCheckHelper.CheckUrlIsAvailable(tobiUrl + "/");
            }
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
