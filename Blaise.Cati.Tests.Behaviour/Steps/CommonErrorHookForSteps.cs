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
                BrowserHelper.OnError(TestContext.CurrentContext, _scenarioContext);
                throw new Exception(_scenarioContext.TestError.Message);
            }
        }
    }
}
