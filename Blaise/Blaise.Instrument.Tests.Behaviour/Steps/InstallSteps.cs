using TechTalk.SpecFlow;

namespace Blaise.Instrument.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class InstallSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;

        public InstallSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
    }
}
