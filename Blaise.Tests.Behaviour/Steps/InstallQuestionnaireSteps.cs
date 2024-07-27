using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Questionnaire;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class InstallQuestionnaireSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        [Given(@"I have an questionnaire I want to use to capture respondents data")]
        public void GivenIHaveAnInstrumentIWantToUseToCaptureRespondentsData()
        {
            var instrumentPackage = BlaiseConfigurationHelper.InstrumentPackage;

            if (string.IsNullOrWhiteSpace(instrumentPackage))
            {
                Assert.Fail("No instrument package has been configured");
            }
        }

        [Given(@"I have an questionnaire installed on a Blaise environment")]
        [Given(@"There is an questionnaire installed on a Blaise environment")]
        [When(@"I install the questionnaire into a Blaise environment")]
        public void WhenIInstallTheInstrumentIntoABlaiseEnvironment()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire();
        }

        [When(@"I install the questionnaire into a Blaise environment specifying a Cati configuration")]
        public void WhenIInstallTheInstrumentIntoABlaiseEnvironmentSpecifyingACatiConfiguration()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire();
        }

        [Then(@"the questionnaire is available to use in the Blaise environment")]
        public void ThenTheInstrumentIsAvailableToUseInTheBlaiseEnvironment()
        {
            var instrumentHasInstalled = QuestionnaireHelper.GetInstance().SurveyHasInstalled(BlaiseConfigurationHelper.InstrumentName, 60);

            Assert.IsTrue(instrumentHasInstalled, "The questionnaire has not been installed, or is not active");
        }

        [Then(@"the questionnaire is configured to capture respondents data via Cati")]
        public void ThenTheInstrumentIsConfiguredToCaptureRespondentsDataViaCati()
        {
            var surveyConfiguration = QuestionnaireHelper.GetInstance().GetSurveyInterviewType();
            Assert.AreEqual(QuestionnaireInterviewType.Cati, surveyConfiguration);
        }

        [AfterScenario("instrument")]
        public void CleanUpScenario()
        {
            QuestionnaireHelper.GetInstance().UninstallSurvey();
        }

        [AfterTestRun]
        public static void CleanUpTestRun()
        {
            BrowserHelper.ClearSessionData();
        }
    }
}
