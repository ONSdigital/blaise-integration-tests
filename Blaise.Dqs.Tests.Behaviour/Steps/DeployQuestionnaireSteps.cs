using System;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.DQS;
using Blaise.Tests.Helpers.Instrument;
using NUnit.Framework;
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
            Assert.AreEqual(DqsConfigurationHelper.DqsUrl, BrowserHelper.CurrentUrl);
        }

        [When(@"I confirm my selection")]
        public void WhenIConfirmMySelection()
        {
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
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
            Assert.AreEqual(DqsConfigurationHelper.QuestionnaireExistsUrl, BrowserHelper.CurrentUrl);
        }

        [Given(@"I have been presented with questionnaire already exists screen")]
        public void GivenIHaveBeenPresentedWithQuestionnaireAlreadyExistsScreen()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
            DqsHelper.GetInstance().LoadUploadPage();
            DqsHelper.GetInstance().SelectQuestionnairePackage();

            Assert.AreEqual(DqsConfigurationHelper.QuestionnaireExistsUrl, BrowserHelper.CurrentUrl);

            _scenarioContext.Set(InstrumentHelper.GetInstance().GetInstallDate(), InstallDate);
        }

        [When(@"I select cancel")]
        public void WhenISelectCancelDeploymentOfQuestionnaire()
        {
            DqsHelper.GetInstance().CancelDeploymentOfQuestionnaire();
        }

        [Then(@"the questionnaire has not been overwritten")]
        public void ThenTheQuestionnaireHasNotBeenOverwritten()
        {
            var expectedInstallDate = _scenarioContext.Get<DateTime>(InstallDate);
            var actualInstallDate = InstrumentHelper.GetInstance().GetInstallDate();

            Assert.AreEqual(expectedInstallDate, actualInstallDate);
        }
        
        [Then(@"the questionnaire is active in blaise")]
        public void ThenTheQuestionnaireIsActiveInBlaise()
        {
            var instrumentInstalled = InstrumentHelper.GetInstance().SurveyHasInstalled(BlaiseConfigurationHelper.InstrumentName, 60);
            Assert.IsTrue(instrumentInstalled);
        }

        [Given(@"the package I have selected already exists in Blaise")]
        public void GivenThePackageIHaveSelectedAlreadyExistsInBlaise()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
        }
        
        [AfterScenario("questionnaire")]
        public void CleanUpScenario()
        {
            BrowserHelper.CloseBrowser();
            InstrumentHelper.GetInstance().UninstallSurvey();
        }
    }
}
