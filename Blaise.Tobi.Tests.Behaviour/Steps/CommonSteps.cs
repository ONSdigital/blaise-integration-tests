namespace Blaise.Tobi.Tests.Behaviour.Steps
{
    using System;
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Case;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework.Extensions;
    using Blaise.Tests.Helpers.Health;
    using Blaise.Tests.Helpers.Questionnaire;
    using Blaise.Tests.Helpers.Tobi;
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

            var tobiUrl = ConfigurationExtensions.TryGetVariable("ENV_TOBI_URL");
            if (!string.IsNullOrEmpty(tobiUrl))
            {
                HealthCheckHelper.CheckUrl(tobiUrl + "/");
            }

            QuestionnaireHelper.GetInstance().InstallQuestionnaire(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName,
                BlaiseConfigurationHelper.QuestionnairePath,
                BlaiseConfigurationHelper.QuestionnaireInstallOptions);

            var primaryKeyValues = new Dictionary<string, string> { { "QID.Serial_Number", "9001" } };
            CaseHelper.GetInstance().CreateCase(new CaseModel(primaryKeyValues, "110", "07000000000"));
            DaybatchHelper.GetInstance().SetSurveyDay(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
            DaybatchHelper.GetInstance().CreateDaybatch(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            DaybatchHelper.GetInstance().RemoveSurveyDays(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
            QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            BrowserHelper.CloseBrowser();
        }

        [AfterStep]
        public void AfterStep()
        {
            if (_scenarioContext.TestError != null)
            {
                BrowserHelper.OnError(TestContext.CurrentContext, _scenarioContext);
            }
        }
    }
}
