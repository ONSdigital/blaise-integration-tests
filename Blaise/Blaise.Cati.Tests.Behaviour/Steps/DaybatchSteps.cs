using System.Collections.Generic;
using System.Linq;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Instrument;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DaybatchSteps
    {
        [BeforeFeature("cati")]
        public static void InitializeFeature()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
        }

        [Given(@"I log on to the Cati Dashboard")]
        public void GivenILogOnToTheCatiDashboard()
        {
            CatiHelper.GetInstance().LogIntoCati();
            Assert.AreNotEqual(CatiConfigurationHelper.LoginUrl, CatiHelper.GetInstance().CurrentUrl(),
                                "Expected to leave the login page");
        }

        [When(@"I create a daybatch for today")]
        public void WhenICreateADaybatchForToday()
        {
            CatiHelper.GetInstance().CreateDayBatch();
        }

        [Then(@"the sample cases are present on the daybatch entry screen")]
        public void ThenTheSampleCasesArePresentOnTheDaybatchEntryScreen(IEnumerable<CaseModel> cases)
        {     
            var entriesText = CatiHelper.GetInstance().GetDaybatchEntriesText();
            var expectedNumberOfCases = cases.Count();
            Assert.AreEqual($"Showing 1 to {expectedNumberOfCases} of {expectedNumberOfCases} entries", entriesText);
        }
                
        [AfterFeature("cati")]
        public static void CleanUpFeature()
        {
            CaseHelper.GetInstance().DeleteCases();
            InstrumentHelper.GetInstance().UninstallSurvey();
        }
    }
}
