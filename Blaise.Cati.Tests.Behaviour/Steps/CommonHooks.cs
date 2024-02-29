using Blaise.Tests.Helpers.Browser;
using NUnit.Framework;
using TechTalk.SpecFlow;
using System;
using Blaise.Tests.Helpers.Case;
using System.Collections.Generic;
using Blaise.Tests.Models.Case;

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

        [Given(@"I have created sample cases for the questionnaire")]
        public void WhenICreateACaseForTheInstrument(IEnumerable<CaseModel> caseModels)
        {
            CaseHelper.GetInstance().DeleteCases();
            CaseHelper.GetInstance().CreateCases(caseModels);
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
