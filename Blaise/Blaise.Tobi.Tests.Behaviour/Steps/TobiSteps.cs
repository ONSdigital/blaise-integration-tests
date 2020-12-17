using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Instrument;
using Blaise.Tests.Helpers.Tobi;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Tobi.Tests.Behaviour.Steps
{
    [Binding]
    public class TobiSteps
    {
        [BeforeFeature("HappyPath")]
        public static void InitializeFeature()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
            CaseHelper.GetInstance().CreateCase(new CaseModel("900000", "110", "07000 000 00"));
            CatiManagementHelper.GetInstance().CreateDayBatch();
        }

        [Given(@"I have internet access")]
        public void GivenIHaveInternetAccess()
        {
        }


        [Given(@"I can view a list of surveys on Blaise within TOBI")]
        [When(@"I launch TOBI")]
        public void WhenILaunchTobi()
        {
            TobiHelper.GetInstance().LoadTobiHomePage();
        }

        [When(@"I select the survey I am working on")]
        public void WhenISelectTheSurveyIAmWorkingOn()
        {
            var currentUrl = TobiHelper.GetInstance().ClickLoadQuestionnaire();
            Assert.AreEqual(TobiConfigurationHelper.SurveyUrl, currentUrl);
        }

        [Then(@"I will be able to view all live surveys with questionnaires loaded in Blaise, identified by their three letter acronym \(TLA\), i\.e\. OPN, LMS")]
        public void ThenIWillBeAbleToViewAllLiveSurveysWithQuestionnairesLoadedInBlaiseIdentifiedByTheirThreeLetterAcronym()
        {
            Assert.AreEqual("OPN", TobiHelper.GetInstance().CheckSurveyIsDisplaying());
        }

        [Then(@"I am presented with a list of active questionnaires to be worked on that day for that survey")]
        public void ThenIAmPresentedWithAListOfActiveQuestionnairesToBeWorkedOnThatDayForThatSurvey()
        {
           Assert.AreEqual(BlaiseConfigurationHelper.InstrumentName, TobiHelper.GetInstance().GetFirstQuestionnaireInTable());
        }
    }
}
