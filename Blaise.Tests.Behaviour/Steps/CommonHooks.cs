using System;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Questionnaire;
using Blaise.Nuget.Api.Contracts.Enums;
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

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            CheckForErroneousQuestionnaire();
        }

        [BeforeScenario(Order = -1)]
        public void BeforeScenario()
        {
            CheckForErroneousQuestionnaire();
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

        private static void CheckForErroneousQuestionnaire()
        {
            var questionnaireHelper = QuestionnaireHelper.GetInstance();
            var questionnaireStatus = questionnaireHelper.GetQuestionnaireStatus();

            if (questionnaireStatus == QuestionnaireStatusType.Erroneous)
            {
                TestContext.WriteLine(@"
                 _____                                            _ 
                |  ___|                                          | |
                | |__ _ __ _ __ ___  _ __   ___  ___  _   _ ___  | |
                |  __| '__| '__/ _ \| '_ \ / _ \/ _ \| | | / __| | |
                | |__| |  | | | (_) | | | |  __/ (_) | |_| \__ \ |_|
                \____/_|  |_|  \___/|_| |_|\___|\___/ \__,_|___/ (_)
                ");
                TestContext.WriteLine("The questionnaire is in an erroneous state. All tests are skipped. Please restart Blaise on the management VM and uninstall it via Blaise Server Manager.");
                Assert.Fail("The questionnaire is in an erroneous state. All tests are skipped.");
            }
        }
    }
}