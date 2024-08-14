using System;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.Questionnaire;
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
            QuestionnaireHelper.GetInstance().InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, BlaiseConfigurationHelper.QuestionnairePath);
        }

        [Given(@"the questionnaire is active")]
        public void GivenTheQuestionnaireIsActive()
        {
            Assert.IsTrue(QuestionnaireHelper.GetInstance().CheckQuestionnaireActive(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName));
        }

        [Given(@"the questionnaire is not active")]
        public void GivenTheQuestionnaireIsNotActive()
        {
            QuestionnaireHelper.GetInstance().DeactivateQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        [Given(@"I select delete on the questionnaire details page")]
        public void GivenISelectDeleteOnAQuestionnaireThatIsNotLive()
        {
            try
            {
                DqsHelper.GetInstance().DeleteQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName);
            }
            catch (Exception e)
            {
                Console.WriteLine($" select delete on the questionnaire details page failed {e}");
                throw;
            }
        }

        [When(@"I select the questionnaire in the list")]
        public void WhenILocateThatQuestionnaireInTheList()
        {
            DqsHelper.GetInstance().LoadDqsHomePage();
            var questionnairesInTable = DqsHelper.GetInstance().GetQuestionnaireTableContents();
            Assert.IsTrue(questionnairesInTable.Any(q => q == BlaiseConfigurationHelper.QuestionnaireName));
            DqsHelper.GetInstance().ClickQuestionnaireInfoButton(BlaiseConfigurationHelper.QuestionnaireName);
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
            if (QuestionnaireHelper.GetInstance().CheckQuestionnaireExists(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName))
            {
                QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
            }
            DqsHelper.GetInstance().LogOutOfToDqs();
            BrowserHelper.ClosePreviousTab();
        }
    }
}
