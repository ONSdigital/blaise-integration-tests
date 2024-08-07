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
                Console.WriteLine(@"
 ______ _____ _____ ____ _ _ ______ ____ _ _ _____
| ____| __ \| __ \ / __ \| \ | | ____/ __ \| | | |/ ____|
| |__ | |__) | |__) | | | | \| | |__ | | | | | | | (___
| __| | _ /| _ /| | | | . ` | __|| | | | | | |\___ \
| |____| | \ \| | \ \| |__| | |\ | |___| |__| | |__| |____) |
|______|_| \_\_| \_\\____/|_| \_|______\____/ \____/|_____/
");
                Console.WriteLine("The questionnaire is in an erroneous state. All tests are skipped. Please restart Blaise on the management VM and uninstall it via Blaise Server Manager.");
                Assert.Fail("The questionnaire is in an erroneous state. All tests are skipped.");
            }
        }
    }
}