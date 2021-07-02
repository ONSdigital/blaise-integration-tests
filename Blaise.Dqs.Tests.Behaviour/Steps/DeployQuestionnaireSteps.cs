using System;
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
            var questionnairesInTable = DqsHelper.GetInstance().GetQuestionnaireTableContents();
            Assert.IsTrue(questionnairesInTable.Any(q => q == BlaiseConfigurationHelper.InstrumentName));
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
            Assert.AreEqual($"{DqsConfigurationHelper.DqsUrl}", BrowserHelper.CurrentUrl);
        }

        [When(@"I confirm my selection")]
        [When(@"I Deploy The Questionnaire")]
        public void WhenIConfirmMySelection()
        {
            try
            {
                DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "NotOverwritten", "Questionnaire has not been Overwritten");
            }
        }

        [When(@"I dont select a Start date")]
        public void WhenIdontSelectAStartDate()
        {
            try
            {
                DqsHelper.GetInstance().SelectNoLiveDate();
                DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "NoStartDateSelected", "Cannot select no Start date");
            }
        }

        [When(@"I set a start date to today")]
        public void WhenISetAStartDateTo()
        {
            try
            {
                var today = DateTime.Now.ToString("dd/MM/yyyy");
                DqsHelper.GetInstance().SelectYesLiveDate();
                DqsHelper.GetInstance().SetLiveDate(today);
                DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "LiveDateFieldNotSetCorrectly", "Start Date Field Not Set Correctly");
            }
        }

        [When(@"The set start date for questionnaire returns today")]
        public void WhenTheSetStartDateForQuestionnaireReturns()
        {
            try
            {
                var today = DateTime.Now.ToString("dd/MM/yyyy");
                Assert.AreEqual($"Start date set to {today}", DqsHelper.GetInstance().GetLivedateSummaryText());
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "LiveDateFieldNotSetCorrectly", "Start Date Field Not Set Correctly");
            }
        }


        [When(@"The set start date for questionnaire returns Start Date Not Specified")]
        public void WhenTheSetStartDateForQuestionnaireReturnsLiveDateNotSpecified()
        {
            try
            {
                Assert.AreEqual("Start date not specified", DqsHelper.GetInstance().GetLivedateSummaryText());
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "StartDateNotSetCorrectly", "Unable to find start date response");
            }
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
            try
            {
                DqsHelper.GetInstance().WaitForUploadToComplete();
                var successMessage = DqsHelper.GetInstance().GetUploadMessage();
                Assert.IsNotNull(successMessage);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "NotOverwritten", "Questionnaire has not been Overwritten");
            }
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
            DqsHelper.GetInstance().ConfirmQuestionnaireUpload();
        }

        [Then(@"the questionnaire has not been overwritten")]
        public void ThenTheQuestionnaireHasNotBeenOverwritten()
        {
            try
            {
                var expectedInstallDate = _scenarioContext.Get<DateTime>(InstallDate);
                var actualInstallDate = InstrumentHelper.GetInstance().GetInstallDate();

                Assert.AreEqual(expectedInstallDate, actualInstallDate);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "NotOverwritten", "Questionnaire has not been Overwritten");
            }
        }

        [Then(@"I am presented with a warning that I cannot overwrite the survey")]
        public void ThenIAmPresentedWithAWarningThatICannotOverwriteTheSurvey()
        {
            try
            {
                Assert.IsNotNull(DqsHelper.GetInstance().GetOverwriteMessage());
                Assert.AreEqual(DqsConfigurationHelper.CannotOverwriteUrl, BrowserHelper.CurrentUrl);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "CannotOverwrite", "Questionnaire Cannot be overwritten");
            }
        }


        [Then(@"Then the questionnaire package is deployed and overwrites the existing questionnaire")]
        public void ThenThenTheQuestionnairePackageIsDeployedAndOverwritesTheExistingQuestionnaire()
        {
            try
            {
                DqsHelper.GetInstance().WaitForUploadToComplete();
                var existingInstallDate = _scenarioContext.Get<DateTime>(InstallDate);
                var newInstallDate = InstrumentHelper.GetInstance().GetInstallDate();

                Assert.IsTrue(newInstallDate > existingInstallDate);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "OverwriteExisting", "Questionnaire has been overwritten");
            }

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
