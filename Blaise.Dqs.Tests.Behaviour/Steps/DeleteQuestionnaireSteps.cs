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
        public void GivenIHaveTheNameOfAQuestionnaireIWantToDeleteAndThatSurveyIsLive()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire();
        }

        [Given(@"the questionnaire is active")]
        public void GivenTheQuestionnaireIsActive()
        {
            Assert.IsTrue(QuestionnaireHelper.GetInstance().SurveyIsActive(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName));
        }

        [Given(@"the questionnaire is not active")]
        public void GivenTheQuestionnaireIsNotActive()
        {
            QuestionnaireHelper.GetInstance().DeactivateSurvey(BlaiseConfigurationHelper.QuestionnaireName,
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
            DqsHelper.GetInstance().ClickInstrumentInfoButton(BlaiseConfigurationHelper.QuestionnaireName);
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
            if (QuestionnaireHelper.GetInstance().SurveyExists(BlaiseConfigurationHelper.QuestionnaireName))
            {
                var caseHelper = CaseHelper.GetInstance();
                caseHelper?.DeleteCases();

                QuestionnaireHelper.GetInstance().UninstallSurvey();
            }
            BrowserHelper.ClosePreviousTab();
        }

    }
}
