using Blaise.Nuget.Api.Contracts.Enums;
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
            _scenarioContext.Set(_configurationHelper.GetInstrumentPackage(), "instrumentPackagePath");
            _scenarioContext.Set(_configurationHelper.InstrumentName, "instrumentName");
        }

        [When(@"I install the instrument into a Blaise environment")]
        public void WhenIInstallTheInstrumentIntoABlaiseEnvironment()
        {
            InstallIntrument();
        }

        [When(@"I install the instrument into a Blaise environment specifying a Cati configuration")]
        public void WhenIInstallTheInstrumentIntoABlaiseEnvironmentSpecifyingACatiConfiguration()
        {
            InstallIntrument(SurveyInterviewType.Cati);
        }

        [Then(@"the instrument is available to use in the Blaise environment")]
        public void ThenTheInstrumentIsAvailableToUseInTheBlaiseEnvironment()
        {
            var instrumentName = _scenarioContext.Get<string>("instrumentName");
            var instrumentHasInstalled = _instrumentHelper.SurveyIsInstalledSuccessfully(instrumentName, 60);

            Assert.IsTrue(instrumentHasInstalled);
        }

        [Then(@"the instrument is configured to capture respondents data via Cati")]
        public void ThenTheInstrumentIsConfiguredToCaptureRespondentsDataViaCati()
        {
            var instrumentName = _scenarioContext.Get<string>("instrumentName");
            var surveyConfiguration = _instrumentHelper.GetSurveyInterviewType(instrumentName);
            Assert.AreEqual(SurveyInterviewType.Cati, surveyConfiguration);
        }

        [AfterScenario]
        public void CleanUp()
        {
            UninstallInstrument();
        }

        private void UninstallInstrument()
        {
            var instrumentName = _scenarioContext.Get<string>("instrumentName");
            _instrumentHelper.UninstallSurvey(instrumentName);
        }

        private void InstallIntrument(SurveyInterviewType surveyType = SurveyInterviewType.Cati)
        {
            var instrumentPackagePath = _scenarioContext.Get<string>("instrumentPackagePath");
            _instrumentHelper.InstallInstrument(instrumentPackagePath, surveyType);
        }

    }
}
