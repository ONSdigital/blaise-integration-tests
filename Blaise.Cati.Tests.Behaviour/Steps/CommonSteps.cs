namespace Blaise.Cati.Tests.Behaviour.Steps
{
    using System;
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Case;
    using Blaise.Tests.Helpers.Cati;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework.Extensions;
    using Blaise.Tests.Helpers.Health;
    using Blaise.Tests.Helpers.Questionnaire;
    using Blaise.Tests.Models.Case;
    using NUnit.Framework;
    using Reqnroll;

    [Binding]
    public sealed class CommonSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            HealthCheckHelper.CheckBlaiseConnection();

            var catiUrl = ConfigurationExtensions.TryGetVariable("ENV_BLAISE_CATI_URL");
            if (!string.IsNullOrEmpty(catiUrl))
            {
                if (!catiUrl.StartsWith("http://") && !catiUrl.StartsWith("https://"))
                {
                    catiUrl = "https://" + catiUrl;
                }

                HealthCheckHelper.CheckUrl(catiUrl);
            }

            QuestionnaireHelper.GetInstance().InstallQuestionnaire(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName,
                BlaiseConfigurationHelper.QuestionnairePath,
                BlaiseConfigurationHelper.QuestionnaireInstallOptions);

            CatiManagementHelper.GetInstance().CreateAdminUser();
            CatiInterviewHelper.GetInstance().CreateInterviewUser();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            CatiInterviewHelper.GetInstance().DeleteInterviewUser();
            CatiManagementHelper.GetInstance().DeleteAdminUser();
            QuestionnaireHelper.GetInstance().UninstallQuestionnaire(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        [AfterStep]
        public void AfterStep()
        {
            if (_scenarioContext.TestError != null)
            {
                BrowserHelper.OnError(TestContext.CurrentContext, _scenarioContext);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            BrowserHelper.CloseBrowser();
        }

        [Given(@"there is a CATI questionnaire installed")]
        public void GivenThereIsACatiQuestionnaireInstalled()
        {
            QuestionnaireHelper.GetInstance().EnsureQuestionnaireReadyForTest(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName);
        }

        [Given(@"I have created cases for the questionnaire")]
        public void GivenIHaveCreatedCasesForTheQuestionnaire(IEnumerable<CaseModel> caseModels)
        {
            CaseHelper.GetInstance().DeleteCases();
            CaseHelper.GetInstance().CreateCases(caseModels);
        }

        [Given(@"I log into the CATI dashboard as an administrator")]
        public void GivenILogIntoTheCatiDashboardAsAnAdministrator()
        {
            CatiManagementHelper.GetInstance().LogIntoCatiDashboardAsAdministrator();
            var currentUrl = CatiManagementHelper.GetInstance().CurrentUrl();

            Assert.That(
                currentUrl,
                Is.Not.EqualTo(CatiConfigurationHelper.LoginUrl).IgnoreCase,
                $"Expected to leave the login page, but current URL is still {currentUrl}");
        }

        [Given(@"I have created a daybatch for today")]
        [When(@"I create a daybatch for today")]
        public void GivenIHaveCreatedADaybatchForToday()
        {
            CatiManagementHelper.GetInstance().ClearDayBatchEntries();
            CatiManagementHelper.GetInstance().CreateDayBatch();
        }
    }
}
