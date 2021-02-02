using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
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
            Assert.AreEqual("Questionnaire is live",DqsHelper.GetInstance().GetTextForDeletion());
        }

        [Then(@"the questionnaire is removed from Blaise")]
        public void ThenTheQuestionnaireIsRemovedFromBlaise()
        {
            DqsHelper.GetInstance().GetDeletionSummary();
            Assert.IsFalse(InstrumentHelper.GetInstance().SurveyExists(BlaiseConfigurationHelper.InstrumentName));
        }

        [AfterScenario("delete")]
        public void CleanUpScenario()
        {
            BrowserHelper.CloseBrowser();
            if (InstrumentHelper.GetInstance().SurveyExists(BlaiseConfigurationHelper.InstrumentName))
                InstrumentHelper.GetInstance().UninstallSurvey();
        }

    }
}
