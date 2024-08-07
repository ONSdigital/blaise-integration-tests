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
        public void GivenIHaveAQuestionnaireIWantToUseToCaptureRespondentsData()
        {
            var questionnairePackage = BlaiseConfigurationHelper.QuestionnairePackage;

            if (string.IsNullOrWhiteSpace(questionnairePackage))
            {
                Assert.Fail("No questionnaire package has been configured");
            }
        }

        [Given(@"I have an questionnaire installed on a Blaise environment")]
        [Given(@"There is an questionnaire installed on a Blaise environment")]
        [When(@"I install the questionnaire into a Blaise environment")]
        [When(@"I install the questionnaire into a Blaise environment specifying a Cati configuration")]
        public void WhenIInstallTheQuestionnaireIntoABlaiseEnvironment()
        {
            var questionnaireHelper = QuestionnaireHelper.GetInstance();
            var questionnaireStatus = questionnaireHelper.GetQuestionnaireStatus();

            if (questionnaireStatus == QuestionnaireStatusType.Erroneous)
            {
                Assert.Fail("The questionnaire is in an erroneous state, please restart Blaise on the management VM and uninstall it via Blaise Server Manager");
            }

            questionnaireHelper.InstallQuestionnaire();

            questionnaireStatus = questionnaireHelper.GetQuestionnaireStatus();

            if (questionnaireStatus == QuestionnaireStatusType.Erroneous)
            {
                Assert.Fail("The questionnaire is in an erroneous state, please restart Blaise on the management VM and uninstall it via Blaise Server Manager");
            }
        }

        [Then(@"the questionnaire is available to use in the Blaise environment")]
        public void ThenTheQuestionnaireIsAvailableToUseInTheBlaiseEnvironment()
        {
            var questionnaireHasInstalled = QuestionnaireHelper.GetInstance().SurveyHasInstalled(BlaiseConfigurationHelper.QuestionnaireName, 60);

            Assert.IsTrue(questionnaireHasInstalled, "The questionnaire has not been installed, or is not active");
        }

        [Then(@"the questionnaire is configured to capture respondents data via Cati")]
        public void ThenTheQuestionnaireIsConfiguredToCaptureRespondentsDataViaCati()
        {
            var surveyConfiguration = QuestionnaireHelper.GetInstance().GetSurveyInterviewType();
            Assert.AreEqual(QuestionnaireInterviewType.Cati, surveyConfiguration);
        }

        [AfterScenario("questionnaire")]
        public void CleanUpScenario()
        {
            var questionnaireHelper = QuestionnaireHelper.GetInstance();
            var questionnaireStatus = questionnaireHelper.GetQuestionnaireStatus();

            if (questionnaireStatus != QuestionnaireStatusType.Erroneous)
            {
                questionnaireHelper.UninstallSurvey();
            }
            else
            {
                Console.WriteLine("Questionnaire is in an erroneous state. Skipping uninstallation. Please restart Blaise on the management VM and uninstall it via Blaise Server Manager.");
            }
        }

        [AfterTestRun]
        public static void CleanUpTestRun()
        {
            BrowserHelper.ClearSessionData();
        }
    }
}
