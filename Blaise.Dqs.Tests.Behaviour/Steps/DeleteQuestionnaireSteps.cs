﻿using System;
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
        public void GivenIHaveAQestionnaireIWantToDelete()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, BlaiseConfigurationHelper.QuestionnairePath);
        }

        [Given(@"the questionnaire is active")]
        public void GivenTheQuestionnaireIsActive()
        {
            Assert.IsTrue(QuestionnaireHelper.GetInstance().CheckQuestionnaireActive(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName));
        }

        [Given(@"I select delete on the questionnaire details page")]
        public void GivenISelectDeleteOnTheQuestionnaireDetailsPage()
        {
            try
            {
                DqsHelper.GetInstance().DeleteQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Selecting delete on the questionnaire details page failed");
                Console.WriteLine($"{e}");
                throw;
            }
        }

        [Given(@"I am taken to the delete confirmation page")]
        public void GivenIAmTakenToTheDeleteConfirmationPage()
        {
            DqsHelper.GetInstance().WaitForDeleteQuestionnaireConfirmationPage();
        }

        [When(@"I confirm that I want to proceed")]
        public void WhenIConfirmThatIWantToProceed()
        {
            DqsHelper.GetInstance().ConfirmDeletionOfQuestionnaire();
            DqsHelper.GetInstance().WaitForDeletionToComplete();
        }

        [Then(@"the questionnaire is removed from Blaise")]
        public void ThenTheQuestionnaireIsRemovedFromBlaise()
        {
            Assert.IsNotNull(DqsHelper.GetInstance().GetDeletionSummary());
        }

        [AfterScenario("delete-questionnaire")]
        public void AfterScenario()
        {            
            if (QuestionnaireHelper.GetInstance().CheckQuestionnaireExists(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName))
            {
                QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
            }
            DqsHelper.GetInstance().LogoutOfDqs();
            BrowserHelper.ClosePreviousTab();
        }
    }
}
