using System.Collections.Generic;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Instrument;
using Blaise.Tests.Models.Case;
using TechTalk.SpecFlow;

namespace Blaise.Cati.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DaybatchSteps
    {
        [BeforeFeature]
        public static void InitializeFeature()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
            CaseHelper.GetInstance().CreateSampleCase();
        }
        
        [When(@"I create a daybatch for the instrument for today")]
        public void WhenICreateADaybatchForTheInstrumentForToday()
        {
        }

        [Then(@"the sample cases are present on the daybatch entry screen")]
        public void ThenTheSampleCasesArePresentOnTheDaybatchEntryScreen(IEnumerable<CaseModel> cases)
        {     
        }
                
        [AfterFeature]
        public static void CleanUpFeature()
        {
            CaseHelper.GetInstance().DeleteCases();
            InstrumentHelper.GetInstance().UninstallSurvey();
        }
    }
}
