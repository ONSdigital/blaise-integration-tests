namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    using System;
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Dqs;
    using Blaise.Tests.Helpers.Framework.Extensions;
    using Blaise.Tests.Helpers.Health;
    using Blaise.Tests.Helpers.Questionnaire;
    using Blaise.Tests.Helpers.User;
    using Blaise.Tests.Models.User;
    using NUnit.Framework;
    using Reqnroll;

    [Binding]
    public sealed class CommonSteps
    {
        private static readonly string _username = $"BDSS-test-user-{Guid.NewGuid()}";
        private static readonly string _password = $"{Guid.NewGuid()}";

        private readonly ScenarioContext _scenarioContext;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            HealthCheckHelper.CheckBlaiseConnection();

            var dqsUrl = ConfigurationExtensions.TryGetVariable("ENV_DQS_URL");
            if (!string.IsNullOrEmpty(dqsUrl))
            {
                HealthCheckHelper.CheckUrl(dqsUrl);
            }

            var userModel = new UserModel
            {
                Username = _username,
                Password = _password,
                Role = "BDSS",
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName,
            };
            UserHelper.GetInstance().CreateUser(userModel);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            UserHelper.GetInstance().RemoveUser(_username);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            QuestionnaireHelper.GetInstance().EnsureQuestionnaireReadyForTest(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);
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
            if (QuestionnaireHelper.GetInstance().CheckQuestionnaireExists(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName))
            {
                QuestionnaireHelper.GetInstance().UninstallQuestionnaire(
                    BlaiseConfigurationHelper.QuestionnaireName,
                    BlaiseConfigurationHelper.ServerParkName);
            }

            DqsHelper.GetInstance().LogoutOfDqs();
            BrowserHelper.CloseBrowser();
        }

        [Given(@"a questionnaire has been deployed")]
        [Given(@"I have a questionnaire I want to delete")]
        public void GivenAQuestionnaireHasBeenDeployed()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName,
                BlaiseConfigurationHelper.QuestionnairePath,
                BlaiseConfigurationHelper.QuestionnaireInstallOptions);
        }
    }
}
