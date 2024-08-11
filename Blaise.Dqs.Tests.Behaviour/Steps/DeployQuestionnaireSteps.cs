using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.Questionnaire;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;

namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DeployQuestionnaireSteps
    {
        private const string InstallDate = "InstallDate";

        private readonly ScenarioContext _scenarioContext;

        public DeployQuestionnaireSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I have launched the Questionnaire Deployment Service")]
        public void GivenIHaveLaunchedTheQuestionnaireDeploymentService()
        {
            DqsHelper.GetInstance().LoadDqsHomePage();
        }

        [Given(@"there is a questionnaire installed in Blaise")]
        public void GivenThereIsAQuestionnaireInstalledInBlaise()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire();
        }

        [Then(@"I am presented with a list of the questionnaires already deployed to Blaise")]
        public void ThenIAmPresentedWithAListOfTheQuestionnairesAlreadyDeployedToBlaise()
        {
            var questionnairesInTable = DqsHelper.GetInstance().GetQuestionnaireTableContents();
            Assert.IsTrue(questionnairesInTable.Any(q => q == BlaiseConfigurationHelper.QuestionnaireName));
        }

        [Given(@"I have selected the questionnaire package I wish to deploy")]
        public void GivenIHaveSelectedTheQuestionnairePackageIWishToDeploy()
        {
            DqsHelper.GetInstance().LoadUploadPage();
            DqsHelper.GetInstance().SelectQuestionnairePackage();
        }

        [When(@"I view the landing page")]
        [Then(@"I am returned to the landing page")]
        public void WhenIViewTheLandingPage()
        {
            DqsHelper.GetInstance().LoadDqsHomePage();
            Assert.AreEqual($"{DqsConfigurationHelper.DqsUrl}/", BrowserHelper.CurrentUrl);
        }

        [When(@"I confirm my selection")]
        [When(@"I Deploy The Questionnaire")]
        public void WhenIConfirmMySelection()
        {
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [When(@"I dont select a Start date")]
        public void WhenIdontSelectAStartDate()
        {
            DqsHelper.GetInstance().SelectNoLiveDate();
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [When(@"I set a start date to today")]
        public void WhenISetAStartDateTo()
        {
            var today = DateTime.Now.ToString("dd/MM/yyyy");
            DqsHelper.GetInstance().SelectYesLiveDate();
            DqsHelper.GetInstance().SetLiveDate(today);
            Thread.Sleep(5000);
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [When(@"The set start date for questionnaire returns today")]
        public void WhenTheSetStartDateForQuestionnaireReturns()
        {
            var today = DateTime.Now.ToString("dd/MM/yyyy");
            Assert.AreEqual($"Start date set to {today}", DqsHelper.GetInstance().GetLivedateSummaryText());
        }


        [When(@"The set start date for questionnaire returns Start Date Not Specified")]
        public void WhenTheSetStartDateForQuestionnaireReturnsLiveDateNotSpecified()
        {
            Assert.AreEqual("Start date not specified", DqsHelper.GetInstance().GetLivedateSummaryText());
        }


        [Then(@"I am presented with an option to deploy a new questionnaire")]
        public void ThenIAmPresentedWithAnOptionToDeployANewQuestionnaire()
        {
            DqsHelper.GetInstance().ClickDeployQuestionnaire();
            Assert.AreEqual(DqsConfigurationHelper.UploadUrl, BrowserHelper.CurrentUrl);
        }

        [Then(@"I am presented with a successful deployment information banner")]
        public void ThenIAmPresentedWithASuccessfulDeploymentInformationBanner()
        {
            DqsHelper.GetInstance().WaitForUploadToComplete();
            var successMessage = DqsHelper.GetInstance().GetUploadMessage();
            Assert.IsNotNull(successMessage);
        }

        [Then(@"I am presented with questionnaire already exists screen")]
        public void ThenIAmPresentedWithQuestionnaireAlreadyExistsScreen()
        {
            DqsHelper.GetInstance().WaitForQuestionnaireAlreadyExistsPage();
        }

        [Given(@"the questionnaire has data records")]
        public void GivenTheQuestionnaireHasDataRecords()
        {
            CaseHelper.GetInstance().CreateCase();
        }

        [Given(@"the questionnaire does not have data records")]
        public void GivenTheQuestionnaireDoesNotHaveDataRecords()
        {
            Assert.AreEqual(0, CaseHelper.GetInstance().NumberOfCasesInQuestionnaire());
        }

        [Given(@"I have been presented with questionnaire already exists screen")]
        public void GivenIHaveBeenPresentedWithQuestionnaireAlreadyExistsScreen()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire();
            DqsHelper.GetInstance().LoadUploadPage();
            DqsHelper.GetInstance().SelectQuestionnairePackage();
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();

            _scenarioContext.Set(QuestionnaireHelper.GetInstance().GetQuestionnaireInstallDate(), InstallDate);
        }

        [When(@"I select to cancel")]
        public void WhenISelectCancelDeploymentOfQuestionnaire()
        {
            DqsHelper.GetInstance().CancelDeploymentOfQuestionnaire();
        }

        [When(@"I select to overwrite")]
        public void WhenISelectToOverwrite()
        {
            DqsHelper.GetInstance().OverwriteQuestionnaire();
        }

        [When(@"confirm my selection")]
        public void WhenConfirmMySelection()
        {
            DqsHelper.GetInstance().ConfirmOverwriteOfQuestionnaire();
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [Then(@"the questionnaire has not been overwritten")]
        public void ThenTheQuestionnaireHasNotBeenOverwritten()
        {
            var expectedInstallDate = _scenarioContext.Get<DateTime>(InstallDate);
            var actualInstallDate = QuestionnaireHelper.GetInstance().GetQuestionnaireInstallDate();

            Assert.AreEqual(expectedInstallDate, actualInstallDate);
        }

        [Then(@"I am presented with a warning that I cannot overwrite the survey")]
        public void ThenIAmPresentedWithAWarningThatICannotOverwriteTheSurvey()
        {
            Assert.IsNotNull(DqsHelper.GetInstance().GetOverwriteMessage());
            Assert.AreEqual(DqsConfigurationHelper.CannotOverwriteUrl, BrowserHelper.CurrentUrl);
        }


        [Then(@"Then the questionnaire package is deployed and overwrites the existing questionnaire")]
        public void ThenThenTheQuestionnairePackageIsDeployedAndOverwritesTheExistingQuestionnaire()
        {
            DqsHelper.GetInstance().WaitForUploadToComplete();
            var existingInstallDate = _scenarioContext.Get<DateTime>(InstallDate);
            var newInstallDate = QuestionnaireHelper.GetInstance().GetQuestionnaireInstallDate();

            Assert.Greater(newInstallDate, existingInstallDate);
        }

        [Then(@"the questionnaire is active in blaise")]
        public void ThenTheQuestionnaireIsActiveInBlaise()
        {
            var questionnaireInstalled = QuestionnaireHelper.GetInstance().CheckQuestionnaireInstalled(BlaiseConfigurationHelper.QuestionnaireName, 60);
            Assert.IsTrue(questionnaireInstalled);
        }

        [Given(@"the package I have selected already exists in Blaise")]
        public void GivenThePackageIHaveSelectedAlreadyExistsInBlaise()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire();
        }

        [AfterScenario("questionnaire")]
        public void CleanUpScenario()
        {
            DqsHelper.GetInstance().LogOutOfToDqs();
            if (QuestionnaireHelper.GetInstance().CheckQuestionnaireExists(BlaiseConfigurationHelper.QuestionnaireName))
            {
                CaseHelper.GetInstance().DeleteCases();
                QuestionnaireHelper.GetInstance().UninstallQuestionnaire();
            }
            BrowserHelper.ClosePreviousTab();
        }

    }
}
