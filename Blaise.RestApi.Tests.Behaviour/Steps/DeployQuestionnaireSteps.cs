using Blaise.Tests.Helpers.Instrument;
using TechTalk.SpecFlow;

namespace Blaise.RestApi.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DeployQuestionnaireSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private const string ApiResponse = "ApiResponse";

        public DeployQuestionnaireSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        

        [AfterScenario("questionnaires")]
        public void CleanUpScenario()
        {
            InstrumentHelper.GetInstance().UninstallSurvey();
        }
    }
}
