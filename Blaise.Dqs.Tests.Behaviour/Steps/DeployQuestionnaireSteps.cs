using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        [Then(@"the questionnaire is active in blaise")]
        public void ThenTheQuestionnaireIsActiveInBlaise()
        {
            var instrumentInstalled = InstrumentHelper.GetInstance().SurveyHasInstalled(BlaiseConfigurationHelper.InstrumentName, 60);
            Assert.IsTrue(instrumentInstalled);
        }

        [AfterScenario("questionnaire")]
        public void CleanUpScenario()
        {
            BrowserHelper.CloseBrowser();
            InstrumentHelper.GetInstance().UninstallSurvey();
        }
    }
}
