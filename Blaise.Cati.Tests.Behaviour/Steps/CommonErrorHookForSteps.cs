using Blaise.Tests.Helpers.Browser;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Tests.Helpers.ErrorHandler
{
    using System;

    [Binding]
    public sealed class CommonErrorHookForSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public CommonErrorHookForSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [AfterStep]
        public void OnError()
        {
            if (_scenarioContext.TestError != null)
            {
                try
                {
                    BrowserHelper.OnError(TestContext.CurrentContext, _scenarioContext);
                }
                catch (Exception)
                {
                    Environment.Exit(1); /*Force tests to stop as we have errored*/
                }
            }
        }
    }
}
