using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Instrument;
using NUnit.Framework;
using System.IO;
using TechTalk.SpecFlow;

namespace Blaise.Tests.Helpers.ErrorHandler
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
        public static void CheckForErroneousInstrument()
        {
            InstrumentHelper.GetInstance().CheckForErroneousInstrument("DST2304Z");
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
