using System.IO;
using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Instrument;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Instrument.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class InstallSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;
        private readonly BlaiseConfigurationHelper _configurationHelper;
        private readonly InstrumentHelper _instrumentHelper;

        public InstallSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _configurationHelper = new BlaiseConfigurationHelper();
            _instrumentHelper = new InstrumentHelper();
        }

        [Given(@"I have an instrument I want to use to capture respondents data")]
        public void GivenIHaveAnInstrumentIWantToUseToCaptureRespondentsData()
        {
            var instrumentPackagePath = Path.Combine(_configurationHelper.InstrumentPath,
                                                _configurationHelper.InstrumentName, 
                                                _configurationHelper.InstrumentExtension);
            _scenarioContext.Set(instrumentPackagePath, "instrumentPackagePath");
            _scenarioContext.Set(_configurationHelper.InstrumentName, "instrumentName");
        }

        [When(@"I install the instrument into a Blaise environment")]
        public void WhenIInstallTheInstrumentIntoABlaiseEnvironment()
        {
            var instrumentPackagePath = _scenarioContext.Get<string>("instrumentPackagePath");
            _instrumentHelper.InstallInstrument(instrumentPackagePath);
        }

        [Then(@"the instrument is available to use in the Blaise environment")]
        public void ThenTheInstrumentIsAvailableToUseInTheBlaiseEnvironment()
        {
            var instrumentName = _scenarioContext.Get<string>("instrumentName");
            var instrumentHasInstalled = _instrumentHelper.CheckInstrumentIsInstalled(instrumentName, 60);

            Assert.IsTrue(instrumentHasInstalled);
        }
    }
}
