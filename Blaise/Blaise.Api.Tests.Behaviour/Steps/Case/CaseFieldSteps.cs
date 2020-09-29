using Blaise.Api.Tests.Behaviour.Builders;
using Blaise.Api.Tests.Behaviour.Enums;
using Blaise.Api.Tests.Behaviour.Helpers;
using Blaise.Nuget.Api.Contracts.Enums;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Api.Tests.Behaviour.Steps.Case
{
    [Binding, Scope(Tag = "case")]
    public sealed class CaseFieldSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;

        private readonly CaseModelBuilder _caseBuilder;
        private readonly CaseDataHelper _caseDataHelper;

        public CaseFieldSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _caseBuilder = new CaseModelBuilder();
            _caseDataHelper = new CaseDataHelper();
        }

        [Given(@"a case exists in blaise where the field '(.*)' has a value of '(.*)'")]
        public void GivenACaseExistsInBlaiseWithTheFieldSetTo(FieldNameType fieldNameType, string value)
        {
            var caseModel = _caseBuilder.BuildCaseWithField(fieldNameType, value);
            _caseDataHelper.CreateCase(caseModel);

            _scenarioContext.Set(caseModel.PrimaryKey, ScenarioContextType.PrimaryKey.ToString());
        }

        [When(@"I retrieve the value of the field '(.*)'")]
        public void WhenIRetrieveTheValueOfTheField(FieldNameType fieldNameType)
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextType.PrimaryKey.ToString());

            var value = _caseDataHelper.GetFieldValue(primaryKey, fieldNameType);

            _scenarioContext.Set(value, ScenarioContextType.FieldValue.ToString());
        }

        [Then(@"the value should be '(.*)'")]
        public void ThenTheValueShouldBe(string expectedValue)
        {
            var actualValue = _scenarioContext.Get<string>(ScenarioContextType.FieldValue.ToString());

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
