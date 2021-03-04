using System;
using System.Linq;
using System.Threading;
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

        [Given(@"that survey is live")]
        public void GivenThatSurveyIsLive()
        {
            DayBatchHelper.GetInstance().SetSurveyDay(BlaiseConfigurationHelper.InstrumentName, DateTime.Today);
        }

        [Given(@"I select Delete on a questionnaire that is not live")]
        public void GivenISelectDeleteOnAQuestionnaireThatIsNotLive()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
            DqsHelper.GetInstance().DeleteQuestionnaire(BlaiseConfigurationHelper.InstrumentName);
        }

        [When(@"I locate that questionnaire in the list")]
        public void WhenILocateThatQuestionnaireInTheList()
        {
            DqsHelper.GetInstance().LoadDqsHomePage();
            var questionnairesInTable = DqsHelper.GetInstance().GetQuestionnaireTableContents();
            Assert.IsTrue(questionnairesInTable.Any(q => q == BlaiseConfigurationHelper.InstrumentName));
        }

        [When(@"I confirm that I want to proceed")]
        public void WhenIConfirmThatIWantToProceed()
        {
            DqsHelper.GetInstance().ConfirmDeletionOfQuestionnaire();
            DqsHelper.GetInstance().WaitForDeletionToComplete();
        }

        [Then(@"I will not have the option to delete displayed")]
        public void ThenIWillNotHaveTheOptionToDeleteDisplayed()
        {
            try
            {
                Assert.AreEqual("Questionnaire is live", DqsHelper.GetInstance().GetTextForDeletion());
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
                //Blaise survey is still present after the api call returns deletion is succesfull due to further clean up
                Thread.Sleep(20000);
                Assert.IsFalse(InstrumentHelper.GetInstance().SurveyExists(BlaiseConfigurationHelper.InstrumentName));
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "UninstalledQuestionnaire", "Questionnaire has been Uninstalled from blaise");
            }
        }

        [AfterScenario("delete")]
        public void CleanUpScenario()
        {
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
