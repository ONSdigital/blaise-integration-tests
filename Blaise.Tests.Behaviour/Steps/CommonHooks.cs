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
        private const string ErroneousQuestionnaireAscii = @"
                 _____                                            _ 
                |  ___|                                          | |
                | |__ _ __ _ __ ___  _ __   ___  ___  _   _ ___  | |
                |  __| '__| '__/ _ \| '_ \ / _ \/ _ \| | | / __| | |
                | |__| |  | | | (_) | | | |  __/ (_) | |_| \__ \ |_|
                \____/_|  |_|  \___/|_| |_|\___|\___/ \__,_|___/ (_)
                ";
        private const string ErroneousQuestionnaireMessage = 
            $"The questionnaire is in an erroneous state.{Environment.NewLine}" +
            $"Skipping Tests.{Environment.NewLine}" +
            $"Restart Blaise and uninstall the erroneous questionnaire via Blaise Server Manager.";

        public CommonHooks(ScenarioContext scenarioContext, QuestionnaireHelper questionnaireHelper)
        {
            _scenarioContext = scenarioContext;
            _questionnaireHelper = questionnaireHelper;
        }

        [BeforeTestRun]
        public static void CheckQuestionnaireStatusBeforeTestRun()
        {
            CheckQuestionnaireStatus();
        }

        [BeforeScenario(Order = -1)]
        public void CheckQuestionnaireStatusBeforeScenario()
        {
            CheckQuestionnaireStatus();
        }

        [AfterStep]
        public void HandleTestError()
        {
            if (_scenarioContext.TestError!= null)
            {
                BrowserHelper.OnError(TestContext.CurrentContext, _scenarioContext);
                TestContext.WriteLine($"Test error: {_scenarioContext.TestError.Message}");
            }
        }

        private static void CheckQuestionnaireStatus()
        {
            var questionnaireStatus = _questionnaireHelper.GetQuestionnaireStatus();

            if (questionnaireStatus == QuestionnaireStatusType.Erroneous)
            {
                Console.WriteLine($"##[error]{ErroneousQuestionnaireAscii}");
                Assert.Fail(ErroneousQuestionnaireMessage);
            }
        }
    }
}