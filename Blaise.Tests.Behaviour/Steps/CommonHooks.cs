using System;
using Blaise.Nuget.Api.Contracts.Enums;
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

        private const string ErroneousQuestionnaireAscii = @"
                 _____                                            _ 
                |  ___|                                          | |
                | |__ _ __ _ __ ___  _ __   ___  ___  _   _ ___  | |
                |  __| '__| '__/ _ \| '_ \ / _ \/ _ \| | | / __| | |
                | |__| |  | | | (_) | | | |  __/ (_) | |_| \__ \ |_|
                \____/_|  |_|  \___/|_| |_|\___|\___/ \__,_|___/ (_)
                ";
        private static readonly string ErroneousQuestionnaireMessage =
        $"The test questionnaire {BlaiseConfigurationHelper.QuestionnaireName} is in an erroneous state.\n" +
        "Restart Blaise and uninstall the erroneous questionnaire via Blaise Server Manager.";

        public CommonHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _questionnaireHelper = new QuestionnaireHelper();
        }

        [OneTimeSetUp]
        public void CheckQuestionnaireStatusBeforeTestRun()
        {
            CheckQuestionnaireStatus(_questionnaireHelper, _scenarioContext);
        }

        [BeforeScenario(Order = -1)]
        public void CheckQuestionnaireStatusBeforeScenario()
        {
            CheckQuestionnaireStatus(_questionnaireHelper, _scenarioContext);
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

        private void CheckQuestionnaireStatus(QuestionnaireHelper questionnaireHelper, ScenarioContext scenarioContext)
        {
            try
            {
                var questionnaireStatus = questionnaireHelper.GetQuestionnaireStatus();

                if (questionnaireStatus == QuestionnaireStatusType.Erroneous)
                {
                    //throw new InvalidOperationException($"{ErroneousQuestionnaireAscii}\n{ErroneousQuestionnaireMessage}");
                    Assert.Fail($"{ErroneousQuestionnaireAscii}{ErroneousQuestionnaireMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while checking the questionnaire status: {ex.Message}");
                Console.WriteLine($"Questionnaire {BlaiseConfigurationHelper.QuestionnaireName} not found, continuing with the tests.");
            }
        }
    }
}