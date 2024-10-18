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

        [Given(@"I have launched DQS")]
        public void GivenIHaveLaunchedDqs()
        {
            DqsHelper.GetInstance().LoadDqsHomePage();
        }

        [Given(@"there is a questionnaire installed in Blaise")]
        public void GivenThereIsAQuestionnaireInstalledInBlaise()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, BlaiseConfigurationHelper.QuestionnairePath, BlaiseConfigurationHelper.QuestionnaireInstallOptions);
        }

        [Then(@"I am presented with a list of deployed questionnaires")]
        public void ThenIAmPresentedWithAListOfDeployedQuestionnaires()
        {
            var questionnairesInTable = DqsHelper.GetInstance().GetQuestionnaireTableContents();

            Assert.That(questionnairesInTable,
                Has.Member(BlaiseConfigurationHelper.QuestionnaireName),
                $"Questionnaire '{BlaiseConfigurationHelper.QuestionnaireName}' should be in the list of deployed questionnaires");
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

            Assert.That(BrowserHelper.CurrentUrl,
                Is.EqualTo($"{DqsConfigurationHelper.DqsUrl}/").IgnoreCase,
                $"Current URL should be the DQS landing page: {DqsConfigurationHelper.DqsUrl}/");
        }

        [When(@"I confirm my selection")]
        [When(@"I deploy the questionnaire")]
        public void WhenIConfirmMySelection()
        {
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [When(@"I dont select a TO start date")]
        public void WhenIDontSelectAToStartDate()
        {
            DqsHelper.GetInstance().SelectNoToStartDate();
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [When(@"I set a TO start date for today")]
        public void WhenISetAToStartDateForToday()
        {
            var today = DateTime.Now.ToString("dd/MM/yyyy");
            DqsHelper.GetInstance().SelectYesLiveDate();
            DqsHelper.GetInstance().SetLiveDate(today);
            Thread.Sleep(5000);
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [When(@"the deployment summary confirms the TO start date for today")]
        public void WhenTheDeploymentSummaryConfirmsTheToStartDateForToday()
        {
            var today = DateTime.Now.ToString("dd/MM/yyyy");
            var expectedSummary = $"Start date set to {today}";
            var actualSummary = DqsHelper.GetInstance().GetToStartDateSummaryText();

            Assert.That(actualSummary,
                Is.EqualTo(expectedSummary).IgnoreCase,
                $"Deployment summary should confirm the TO start date as '{expectedSummary}', but got '{actualSummary}'");
        }

        [When(@"the deployment summary confirms no TO start date")]
        public void WhenTheDeploymentSummaryConfirmsNoToStartDate()
        {
            var expectedSummary = "Start date not specified";
            var actualSummary = DqsHelper.GetInstance().GetToStartDateSummaryText();

            Assert.That(actualSummary,
                Is.EqualTo(expectedSummary).IgnoreCase,
                $"Deployment summary should confirm no TO start date with '{expectedSummary}', but got '{actualSummary}'");
        }

        [Then(@"I have the option to deploy a questionnaire")]
        public void ThenIHaveTheOptionToDeployAQuestionnaire()
        {
            DqsHelper.GetInstance().ClickDeployQuestionnaire();

            Assert.That(BrowserHelper.CurrentUrl,
                Is.EqualTo(DqsConfigurationHelper.UploadUrl).IgnoreCase,
                $"After clicking 'Deploy Questionnaire', the current URL should be the upload URL: {DqsConfigurationHelper.UploadUrl}");
        }

        [Then(@"I am presented with a successful deployment information banner")]
        public void ThenIAmPresentedWithASuccessfulDeploymentInformationBanner()
        {
            DqsHelper.GetInstance().WaitForUploadToComplete();
            var successMessage = DqsHelper.GetInstance().GetUploadMessage();

            Assert.That(successMessage,
                Is.Not.Null.And.Not.Empty,
                "Success message should be displayed after the deployment is complete");
        }

        [Given(@"the questionnaire does not have data")]
        public void GivenTheQuestionnaireDoesNotHaveData()
        {
            var numberOfCases = CaseHelper.GetInstance().NumberOfCasesInQuestionnaire();

            Assert.That(numberOfCases,
                Is.EqualTo(0),
                $"Questionnaire should have no cases, but found {numberOfCases} case(s)");
        }

        [Given(@"I have been presented with questionnaire already exists screen")]
        public void GivenIHaveBeenPresentedWithQuestionnaireAlreadyExistsScreen()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, BlaiseConfigurationHelper.QuestionnairePath, BlaiseConfigurationHelper.QuestionnaireInstallOptions);
            DqsHelper.GetInstance().LoadUploadPage();
            DqsHelper.GetInstance().SelectQuestionnairePackage();
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
            _scenarioContext.Set(QuestionnaireHelper.GetInstance().GetQuestionnaireInstallDate(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName), InstallDate);
        }

        [When(@"I select cancel")]
        public void WhenISelectCancel()
        {
            DqsHelper.GetInstance().CancelDeploymentOfQuestionnaire();
        }

        [When(@"I select overwrite")]
        public void WhenISelectOverwrite()
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
            var actualInstallDate = QuestionnaireHelper.GetInstance().GetQuestionnaireInstallDate(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);

            Assert.That(actualInstallDate,
                Is.EqualTo(expectedInstallDate).Within(TimeSpan.FromSeconds(1)),
                $"Questionnaire install date should not have changed. Expected: {expectedInstallDate}, but was: {actualInstallDate}");
        }

        [Then(@"the questionnaire is deployed and overwrites the existing questionnaire")]
        public void ThenTheQuestionnaireIsDeployedAndOverwritesTheExistingQuestionnaire()
        {
            DqsHelper.GetInstance().WaitForUploadToComplete();
            var existingInstallDate = _scenarioContext.Get<DateTime>(InstallDate);
            var newInstallDate = QuestionnaireHelper.GetInstance().GetQuestionnaireInstallDate(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);

            Assert.That(newInstallDate,
                Is.GreaterThan(existingInstallDate),
                $"New install date ({newInstallDate}) should be later than the existing install date ({existingInstallDate}), indicating that the questionnaire has been overwritten");
        }

        [Then(@"the questionnaire is active in Blaise")]
        public void ThenTheQuestionnaireIsActiveInBlaise()
        {
            var questionnaireInstalled = QuestionnaireHelper.GetInstance().CheckQuestionnaireInstalled(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName,
                60);

            Assert.That(questionnaireInstalled,
                Is.True,
                $"Questionnaire '{BlaiseConfigurationHelper.QuestionnaireName}' should be active in Blaise on server park '{BlaiseConfigurationHelper.ServerParkName}'");
        }

        [AfterScenario("deploy-questionnaire")]
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
