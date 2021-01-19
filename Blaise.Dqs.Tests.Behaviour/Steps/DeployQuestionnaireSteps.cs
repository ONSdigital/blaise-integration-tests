﻿using System;
using System.Linq;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Instrument;
using Blaise.Tests.Helpers.Dqs;
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

        [Given(@"there is a questionnaire installed in Blaise")]
        public void GivenThereIsAQuestionnaireInstalledInBlaise()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
        }

        [Then(@"I am presented with a list of the questionnaires already deployed to Blaise")]
        public void ThenIAmPresentedWithAListOfTheQuestionnairesAlreadyDeployedToBlaise()
        {
            Assert.AreEqual(BlaiseConfigurationHelper.InstrumentName.ToLower(), 
                DqsHelper.GetInstance().GetQuestionnaireTableContents().FirstOrDefault().ToLower());
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
            Assert.AreEqual(0, CaseHelper.GetInstance().NumberOfCasesInInstrument());
        }

        [Given(@"I have been presented with questionnaire already exists screen")]
        public void GivenIHaveBeenPresentedWithQuestionnaireAlreadyExistsScreen()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
            DqsHelper.GetInstance().LoadUploadPage();
            DqsHelper.GetInstance().SelectQuestionnairePackage();
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
            DqsHelper.GetInstance().WaitForQuestionnaireAlreadyExistsPage();

            _scenarioContext.Set(InstrumentHelper.GetInstance().GetInstallDate(), InstallDate);
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
            DqsHelper.GetInstance().ConfirmSelection();
        }

        [Then(@"the questionnaire has not been overwritten")]
        public void ThenTheQuestionnaireHasNotBeenOverwritten()
        {
            var expectedInstallDate = _scenarioContext.Get<DateTime>(InstallDate);
            var actualInstallDate = InstrumentHelper.GetInstance().GetInstallDate();

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
            var newInstallDate = InstrumentHelper.GetInstance().GetInstallDate();

            Assert.IsTrue(newInstallDate > existingInstallDate);
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
            if (InstrumentHelper.GetInstance().SurveyExists(BlaiseConfigurationHelper.InstrumentName))
                InstrumentHelper.GetInstance().UninstallSurvey();
        }
    }
}
