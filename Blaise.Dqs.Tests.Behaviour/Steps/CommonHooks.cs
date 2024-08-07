using System;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Questionnaire;
using Blaise.Nuget.Api.Contracts.Enums;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class CommonHooks
    {
        private readonly ScenarioContext _scenarioContext;

        public CommonHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Console.WriteLine("BeforeTestRun hook is running...");
            CheckForErroneousQuestionnaire();
        }

        [BeforeScenario(Order = -1)]
        public void BeforeScenario()
        {
            Console.WriteLine("BeforeScenario hook is running...");
            CheckForErroneousQuestionnaire();
        }

        [AfterStep]
        public void OnError()
        {
            if (_scenarioContext.TestError != null)
            {
                BrowserHelper.OnError(TestContext.CurrentContext, _scenarioContext);
            }
        }
    }
}
