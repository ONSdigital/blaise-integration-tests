namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    using System;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Dqs;
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

                HealthCheckHelper.CheckUrl(catiUrl);
            }

            var dqsUrl = ConfigurationExtensions.TryGetVariable("ENV_DQS_URL");
            if (!string.IsNullOrEmpty(dqsUrl))
            {
                HealthCheckHelper.CheckUrl(dqsUrl);
            }

            var tobiUrl = ConfigurationExtensions.TryGetVariable("ENV_TOBI_URL");
            if (!string.IsNullOrEmpty(tobiUrl))
            {
                HealthCheckHelper.CheckUrl(tobiUrl + "/");
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
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (QuestionnaireHelper.GetInstance().CheckQuestionnaireExists(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName))
            {
                QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
            }

            // It's better to attempt cleanup regardless of failure
            DqsHelper.GetInstance().LogoutOfDqs();
            BrowserHelper.ClosePreviousTab();
        }
    }
}
