using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.Instrument;
using NUnit.Framework;
using System;
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
            try
            {
                var instrumentHelper = InstrumentHelper.GetInstance();
                instrumentHelper.InstallInstrument();
                var dqsHelper = DqsHelper.GetInstance();
                dqsHelper.DeleteQuestionnaire(BlaiseConfigurationHelper.InstrumentName);
            }
            catch (Exception ex)
            {
                FailWithScreenShot(ex, "GivenISelectDeleteOnAQuestionnaireThatIsNotLive", $"An error occurred while deleting the questionnaire. {ex.Message}");
            }
        }

        [When(@"I select the questionnaire in the list")]
        public void WhenILocateThatQuestionnaireInTheList()
        {
            try
            {
                var dqsHelper = DqsHelper.GetInstance();
                dqsHelper.LoadDqsHomePage();
                var questionnairesInTable = dqsHelper.GetQuestionnaireTableContents();

                if (!questionnairesInTable.Contains(BlaiseConfigurationHelper.InstrumentName))
                {
                    // Throw an exception if the questionnaire is not found
                    throw new Exception($"The questionnaire '{BlaiseConfigurationHelper.InstrumentName}' was not found.");
                }
                dqsHelper.ClickInstrumentInfoButton(BlaiseConfigurationHelper.InstrumentName);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "WhenILocateThatQuestionnaireInTheList", $"The questionnaire '{BlaiseConfigurationHelper.InstrumentName}' was not found.");
            }
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
            try
            {
                var dqsHelper = DqsHelper.GetInstance();
                dqsHelper.ConfirmDeletionOfQuestionnaire();
                dqsHelper.WaitForDeletionToComplete();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "WhenIConfirmThatIWantToProceed", $"An error occurred while confirming the deletion of the questionnaire. {e.Message}");
            }
        }

        [Then(@"I will have the option to delete the questionnaire")]
        public void ThenIWillHaveTheOptionToDeleteTheQuestionnaire()
        {
            try
            {
                DqsHelper.GetInstance().CanDeleteQuestionnaire();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "CantDelete", "Questionnaire cannot be deleted");
            }
        }

        [Then(@"the questionnaire is removed from Blaise")]
        public void ThenTheQuestionnaireIsRemovedFromBlaise()
        {
            try
            {
                var dqsHelper = DqsHelper.GetInstance();
                var deletionSummary = dqsHelper.GetDeletionSummary();
                Assert.IsNotNull(deletionSummary);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "UninstalledQuestionnaire", "An error occurred while checking the deletion summary");
            }
        }

        [AfterScenario("delete")]
        public void CleanUpScenario()
        {
            DqsHelper.GetInstance().LogOutOfToDqs();
            BrowserHelper.CloseBrowser();
            if (InstrumentHelper.GetInstance().SurveyExists(BlaiseConfigurationHelper.InstrumentName))
            {
                CaseHelper.GetInstance().DeleteCases();
                InstrumentHelper.GetInstance().UninstallSurvey();
            }
        }

        private static void FailWithScreenShot(Exception e, string screenShotName, string screenShotDescription)
        {
            var screenShotFile = BrowserHelper.TakeScreenShot(TestContext.CurrentContext.WorkDirectory,
                screenShotName);

            TestContext.AddTestAttachment(screenShotFile, screenShotDescription);
            Assert.Fail($"The test failed to complete - {e.Message}");
        }

    }
}
