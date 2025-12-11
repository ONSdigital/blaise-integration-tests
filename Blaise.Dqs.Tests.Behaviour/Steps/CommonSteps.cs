namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Dqs;
    using Blaise.Tests.Helpers.Questionnaire;
    using NUnit.Framework;
    using Reqnroll;
    using System;

    [Binding]
    public sealed class CommonSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
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
