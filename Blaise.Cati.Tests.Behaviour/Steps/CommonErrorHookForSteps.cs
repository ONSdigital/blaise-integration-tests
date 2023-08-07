using Blaise.Tests.Helpers.Browser;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Tests.Helpers.ErrorHandler
{
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
                var screenShotFile = BrowserHelper.TakeScreenShot(TestContext.CurrentContext.WorkDirectory,
                    _scenarioContext.StepContext.StepInfo.Text);
                TestContext.AddTestAttachment(screenShotFile, _scenarioContext.StepContext.StepInfo.Text);
                var htmlFile = BrowserHelper.CurrentWindowHTML();
                TestContext.WriteLine(htmlFile);
            }
        }
    }
}
