using System;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Questionnaire;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class CommonHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly QuestionnaireHelper _questionnaireHelper;

        private static bool _hasFailureOccurred = false;

        public CommonHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _questionnaireHelper = QuestionnaireHelper.GetInstance();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            if (_hasFailureOccurred)
            {
                Assert.Fail("A previous scenario has failed. Stopping tests.");
            }

            var questionnaireName = BlaiseConfigurationHelper.QuestionnaireName;
            var serverParkName = BlaiseConfigurationHelper.ServerParkName;

            bool questionnaireExists = _questionnaireHelper.CheckQuestionnaireExists(questionnaireName, serverParkName);

            if (questionnaireExists)
            {
                _questionnaireHelper.UninstallQuestionnaire(questionnaireName, serverParkName);
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