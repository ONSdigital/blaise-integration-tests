using System;
using System.Linq;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.Instrument;
using Blaise.Tests.Helpers.Tobi;
using NUnit.Framework;
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
                InstrumentHelper.GetInstance().InstallInstrument();
                DqsHelper.GetInstance().DeleteQuestionnaire(BlaiseConfigurationHelper.InstrumentName);

            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "CantDelete", "Questionnaire Cannot be deleted");
            }
        }

        [When(@"I select the questionnaire in the list")]
        public void WhenILocateThatQuestionnaireInTheList()
        {
            try
            {
                DqsHelper.GetInstance().LoadDqsHomePage();
                var questionnairesInTable = DqsHelper.GetInstance().GetQuestionnaireTableContents();
                Assert.IsTrue(questionnairesInTable.Any(q => q == BlaiseConfigurationHelper.InstrumentName));
                DqsHelper.GetInstance().ClickInstrumentInfoButton(BlaiseConfigurationHelper.InstrumentName);

            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "CantDelete", "Questionnaire Cannot be deleted");
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
                DqsHelper.GetInstance().ConfirmDeletionOfQuestionnaire();
                DqsHelper.GetInstance().WaitForDeletionToComplete();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "CantDelete", "Questionnaire Cannot be deleted");
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
                FailWithScreenShot(e, "CantDelete", "Questionnaire Cannot be deleted");
            }
        }

        [Then(@"the questionnaire is removed from Blaise")]
        public void ThenTheQuestionnaireIsRemovedFromBlaise()
        {
            try
            {
                Assert.IsNotNull(DqsHelper.GetInstance().GetDeletionSummary());
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "UninstalledQuestionnaire", "Questionnaire has been Uninstalled from blaise");
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
