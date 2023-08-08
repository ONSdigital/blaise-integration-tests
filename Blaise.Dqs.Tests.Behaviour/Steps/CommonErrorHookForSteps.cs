using Blaise.Tests.Helpers.Browser;
using NUnit.Framework;
using System.IO;
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
                File.WriteAllText(TestContext.CurrentContext.WorkDirectory + ".html", htmlFile);
            }
        }
    }
}
