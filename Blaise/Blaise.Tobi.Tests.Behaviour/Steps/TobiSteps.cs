using System.Linq;
using Blaise.Tests.Helpers.Browser;
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
            CatiManagementHelper.GetInstance().LogIntoCatiManagementPortal();
            CatiManagementHelper.GetInstance().CreateDayBatch(BlaiseConfigurationHelper.InstrumentName);
        }

        [Given(@"I have internet access")]
        public void GivenIHaveInternetAccess()
        {
        }

        [Given(@"I can view a list of surveys on Blaise within TOBI")]
        [Given(@"a survey questionnaire end date has passed")]
        [When(@"I launch TOBI")]
        public void WhenILaunchTobi()
        {
            TobiHelper.GetInstance().LoadTobiHomePage();
        }

        [Given(@"I can view a list of live questionnaires for the survey I am allocated to")]
        public void GivenICanViewAListOfLiveQuestionnairesForTheSurveyIAmAllocatedTo()
        {
            TobiHelper.GetInstance().LoadSurveyPage();
        }

        [Given(@"Another survey is active")]
        public void GivenAnotherSurveyIsActive()
        {
            //InstrumentHelper.GetInstance().InstallInstrument(BlaiseConfigurationHelper.SecondInstrumentPackage);
            CatiManagementHelper.GetInstance().LogIntoCatiManagementPortal();
            CatiManagementHelper.GetInstance().CreateDayBatch(BlaiseConfigurationHelper.SecondInstrumentName);
        }

        [When(@"I select the survey I am working on")]
        public void WhenISelectTheSurveyIAmWorkingOn()
        {
            TobiHelper.GetInstance().LoadTobiHomePage();
            var currentUrl = TobiHelper.GetInstance().ClickLoadQuestionnaire();
            Assert.AreEqual(TobiConfigurationHelper.SurveyUrl, currentUrl);
        }

        [When(@"I select a link to interview against the questionnaire with the survey dates I am working on")]
        public void WhenISelectALinkToInterviewAgainstTheQuestionnaireWithTheSurveyDatesIAmWorkingOn()
        {
            TobiHelper.GetInstance().ClickInterviewButton();
        }

        [Then(@"I will be able to view all live surveys with questionnaires loaded in Blaise, identified by their three letter acronym \(TLA\), i\.e\. OPN, LMS")]
        public void ThenIWillBeAbleToViewAllLiveSurveysWithQuestionnairesLoadedInBlaiseIdentifiedByTheirThreeLetterAcronym()
        {
            Assert.AreEqual("OPN", TobiHelper.GetInstance().GetSurveyTableContents().FirstOrDefault());
        }

        [Then(@"I am presented with a list of active questionnaires to be worked on that day for that survey")]
        public void ThenIAmPresentedWithAListOfActiveQuestionnairesToBeWorkedOnThatDayForThatSurvey()
        {
            Assert.AreEqual(BlaiseConfigurationHelper.InstrumentName, TobiHelper.GetInstance().GetFirstQuestionnaireInTable());
        }

        [Then(@"I am presented with the Blaise log in")]
        public void ThenIAmPresentedWithTheBlaiseLogIn()
        {
            CatiInterviewHelper.GetInstance().GetCaseIdText();
            Assert.AreEqual(BrowserHelper.CurrentUrl, CatiConfigurationHelper.LoginUrl);
        }

        [Then(@"I will not see that questionnaire listed for the survey")]
        public void ThenIWillNotSeeThatQuestionnaireListedForTheSurvey()
        {
            var questionnaireShowing = TobiHelper.GetInstance().GetQuestionnaireTableContents()
                .Where(s => s.Contains(BlaiseConfigurationHelper.InstrumentName));
            Assert.IsEmpty(questionnaireShowing);
        }

        [Then(@"I will not see any surveys listed")]
        public void ThenIWillNotSeeAnySurveysListed()
        {
            Assert.IsEmpty(TobiHelper.GetInstance().GetSurveyTableContents());
        }
    }
}
