using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.Instrument;
using NUnit.Framework;
using System.Linq;
using TechTalk.SpecFlow;

namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    [Binding]
    public class DeleteQuestionnaireSteps
    {

        [Given(@"I have a questionnaire I want to delete")]
        public void GivenIHaveTheNameOfAQuestionnaireIWantToDeleteAndThatSurveyIsLive()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
        }

        [Given(@"the questionnaire is active")]
        public void GivenTheQuestionnaireIsActive()
        {
            Assert.IsTrue(InstrumentHelper.GetInstance().SurveyIsActive(BlaiseConfigurationHelper.InstrumentName, BlaiseConfigurationHelper.ServerParkName));
        }

        [Given(@"the questionnaire is not active")]
        public void GivenTheQuestionnaireIsNotActive()
        {
            InstrumentHelper.GetInstance().DeactivateSurvey(BlaiseConfigurationHelper.InstrumentName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        [Given(@"I select delete on the questionnaire details page")]
        public void GivenISelectDeleteOnAQuestionnaireThatIsNotLive()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
            DqsHelper.GetInstance().DeleteQuestionnaire(BlaiseConfigurationHelper.InstrumentName);
        }

        [When(@"I select the questionnaire in the list")]
        public void WhenILocateThatQuestionnaireInTheList()
        {
            DqsHelper.GetInstance().LoadDqsHomePage();
            var questionnairesInTable = DqsHelper.GetInstance().GetQuestionnaireTableContents();
            Assert.IsTrue(questionnairesInTable.Any(q => q == BlaiseConfigurationHelper.InstrumentName));
            DqsHelper.GetInstance().ClickInstrumentInfoButton(BlaiseConfigurationHelper.InstrumentName);
        }

        [When(@"I am taken to the questionnaire details page")]
        public void WhenIAmTakenToTheQuestionnaireDetailsPage()
        {
            DqsHelper.GetInstance().WaitForQuestionnaireDetailsPage();
        }

        [Given(@"I am taken to the delete confirmation screen")]
        public void GivenIAmPresentedWithTheConfirmationScreen()
        {
            DqsHelper.GetInstance().WaitForDeleteQuestionnaireConfirmationPage();
        }

        [When(@"I confirm that I want to proceed")]
        public void WhenIConfirmThatIWantToProceed()
        {
            DqsHelper.GetInstance().ConfirmDeletionOfQuestionnaire();
            DqsHelper.GetInstance().WaitForDeletionToComplete();
        }

        [Then(@"I will have the option to delete the questionnaire")]
        public void ThenIWillHaveTheOptionToDeleteTheQuestionnaire()
        {
            DqsHelper.GetInstance().CanDeleteQuestionnaire(); 
        }

        [Then(@"the questionnaire is removed from Blaise")]
        public void ThenTheQuestionnaireIsRemovedFromBlaise()
        {
            Assert.IsNotNull(DqsHelper.GetInstance().GetDeletionSummary());
        }

        [AfterScenario("delete")]
        public void CleanUpScenario()
        {
            DqsHelper.GetInstance().LogOutOfToDqs();
            if (InstrumentHelper.GetInstance().SurveyExists(BlaiseConfigurationHelper.InstrumentName))
            {
                var caseHelper = CaseHelper.GetInstance();
                caseHelper?.DeleteCases();

                var instrumentHelper = InstrumentHelper.GetInstance();
                instrumentHelper?.UninstallSurvey();
            }
            BrowserHelper.ClosePreviousTab();
        }

    }
}
