﻿using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Questionnaire;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class InstallQuestionnaireSteps
    {
        [Given(@"I have a questionnaire I want to install")]
        public void GivenIHaveAQuestionnaireIWantToInstall()
        {
            var questionnairePackage = BlaiseConfigurationHelper.QuestionnairePackage;

            if (string.IsNullOrWhiteSpace(questionnairePackage))
            {
                Assert.Fail("No questionnaire package has been configured");
            }
        }

        [Given(@"there is a questionnaire installed")]
        [Given(@"There is an questionnaire installed on a Blaise environment")]
        [When(@"I install the questionnaire")]
        public void GivenThereIsAQuestionnaireInstalled()
        {
            QuestionnaireHelper.GetInstance().InstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, BlaiseConfigurationHelper.QuestionnairePath);
        }

        [Then(@"the questionnaire is available")]
        public void ThenTheQuestionnaireIsAvailable()
        {
            var questionnaireHasInstalled = QuestionnaireHelper.GetInstance().CheckQuestionnaireInstalled(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName, 60);

            Assert.IsTrue(questionnaireHasInstalled, "The questionnaire has not been installed, or is not active");
        }

        [Then(@"the questionnaire is configured to capture respondents data via Cati")]
        public void ThenTheQuestionnaireIsConfiguredToCaptureRespondentsDataViaCati()
        {
            var surveyConfiguration = QuestionnaireHelper.GetInstance().GetQuestionnaireInterviewType(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
            Assert.AreEqual(QuestionnaireInterviewType.Cati, surveyConfiguration);
        }

        [AfterScenario("deploy-questionnaire")]
        public void AfterScenario()
        {
            QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            BrowserHelper.ClearSessionData();
        }
    }
}