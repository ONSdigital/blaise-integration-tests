using System;
using Blaise.Tests.Helpers.Browser;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Tobi.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class CommonSteps
    {
        private readonly ScenarioContext _scenarioContext;

        private static bool _hasFailureOccurred = false;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            if (_hasFailureOccurred)
            {
                Assert.Fail("A previous scenario has failed. Skipping test.");
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
