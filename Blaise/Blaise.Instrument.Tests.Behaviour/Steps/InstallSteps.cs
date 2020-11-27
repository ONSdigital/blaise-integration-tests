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

        [Given(@"I have an instrument I want to use to capture respondents data")]
        public void GivenIHaveAnInstrumentIWantToUseToCaptureRespondentsData()
        {
            var instrumentPackage = BlaiseConfigurationHelper.GetInstance().InstrumentPackage();

            if (string.IsNullOrWhiteSpace(instrumentPackage))
            {
                Assert.Fail("No instrument package has been configured");
            }
        }

        [When(@"I install the instrument into a Blaise environment")]
        public void WhenIInstallTheInstrumentIntoABlaiseEnvironment()
        {
            InstallInstrument();
        }

        [When(@"I install the instrument into a Blaise environment specifying a Cati configuration")]
        public void WhenIInstallTheInstrumentIntoABlaiseEnvironmentSpecifyingACatiConfiguration()
        {
            InstallInstrument(SurveyInterviewType.Cati);
        }

        [Given(@"I have an instrument installed on a Blaise environment")]
        [Then(@"the instrument is available to use in the Blaise environment")]
        public void ThenTheInstrumentIsAvailableToUseInTheBlaiseEnvironment()
        {
            var instrumentHasInstalled = InstrumentHelper.GetInstance().SurveyHasInstalled(60);

            Assert.IsTrue(instrumentHasInstalled, "The instrument has not been installed, or is not active");
        }

        [Then(@"the instrument is configured to capture respondents data via Cati")]
        public void ThenTheInstrumentIsConfiguredToCaptureRespondentsDataViaCati()
        {
            var surveyConfiguration = InstrumentHelper.GetInstance().GetSurveyInterviewType();
            Assert.AreEqual(SurveyInterviewType.Cati, surveyConfiguration);
        }

        [AfterScenario]
        public void CleanUpScenario()
        {
            UninstallInstrument();
        }

        private void UninstallInstrument()
        {
            InstrumentHelper.GetInstance().UninstallSurvey();
        }

        private void InstallInstrument(SurveyInterviewType surveyType = SurveyInterviewType.Cati)
        {
            InstrumentHelper.GetInstance().InstallInstrument(surveyType);
        }
    }
}
