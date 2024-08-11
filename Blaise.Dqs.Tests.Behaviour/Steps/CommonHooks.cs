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

        public CommonHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _questionnaireHelper = QuestionnaireHelper.GetInstance();
        }

        [BeforeScenario]
        public void CheckAndRemoveQuestionnaire()
        {
            var questionnaireName = BlaiseConfigurationHelper.QuestionnaireName;
            var serverParkName = BlaiseConfigurationHelper.ServerParkName;

            bool questionnaireExists = _questionnaireHelper.CheckQuestionnaireExists(questionnaireName, serverParkName);

            if (questionnaireExists)
            {
                _questionnaireHelper.UninstallQuestionnaire(questionnaireName, serverParkName);
                bool questionnaireStillExists = _questionnaireHelper.CheckQuestionnaireExists(questionnaireName, serverParkName);

                if (questionnaireStillExists)
                {
                    Assert.Fail($"Failed to uninstall questionnaire {questionnaireName}");
                }
            }
        }

        [AfterStep]
        public void OnError()
        {
            if (_scenarioContext.TestError != null)
            {
                BrowserHelper.OnError(TestContext.CurrentContext, _scenarioContext);
                throw new Exception(_scenarioContext.TestError.Message);
            }
        }
    }
}