using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Tests.Behaviour.Builders;
using Blaise.Api.Tests.Behaviour.Enums;
using Blaise.Api.Tests.Behaviour.Helpers;
using Blaise.Api.Tests.Behaviour.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Api.Tests.Behaviour.Steps.Case
{
    [Binding, Scope(Tag = "case")]
    public sealed class CaseSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;

        private readonly CaseModelBuilder _caseBuilder;
        private readonly CaseDataHelper _caseDataHelper;
        
        public CaseSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _caseBuilder = new CaseModelBuilder();
            _caseDataHelper = new CaseDataHelper();
        }

        [BeforeScenario]
        public void CleanUpDataBefore()
        {
            CleanUpData();
        }

        [Given(@"I have a new case I want to create")]
        public void GivenIHaveANewCaseIWantToCreate()
        {
            var caseModel = _caseBuilder.BuildBasicCase();

            _scenarioContext.Set(caseModel, ScenarioContextType.CaseModel.ToString());
        }

        [Given(@"a case exists in blaise with the primary key '(.*)'")]
        public void GivenACaseExistsInBlaiseWithThePrimaryKey(string primaryKey)
        {
            var caseModel = _caseBuilder.BuildCaseWithPrimaryKey(primaryKey);
            _caseDataHelper.CreateCase(caseModel);

            _scenarioContext.Set(primaryKey, ScenarioContextType.PrimaryKey.ToString());
        }

        [Given(@"a case does not exist in blaise with the primary key '(.*)'")]
        public void GivenACaseDoesNotExistInBlaiseWithThePrimaryKey(string primaryKey)
        {
            _scenarioContext.Set(primaryKey, ScenarioContextType.PrimaryKey.ToString());
        }
        
        [Given(@"a case exists in blaise with the following data")]
        public void GivenACaseExistsInBlaiseWithTheFollowingData(Dictionary<string, string> fieldData)
        {
            var caseModel = _caseBuilder.BuildCaseWithData(fieldData);
            _caseDataHelper.CreateCase(caseModel);

            _scenarioContext.Set(caseModel.PrimaryKey, ScenarioContextType.PrimaryKey.ToString());
        }

        [Given(@"there are a number of existing cases in Blaise")]
        public void GivenThereAreANumberOfExistingCasesInBlaise(IEnumerable<string> primaryKeys)
        {
            foreach (var primaryKey in primaryKeys)
            {
                _caseDataHelper.CreateCase(_caseBuilder.BuildCaseWithPrimaryKey(primaryKey));
            }
        }
        
        [When(@"I update the case with the following data")]
        public void WhenICallTheUpdateMethodForTheCase(Dictionary<string, string> fieldData)
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextType.PrimaryKey.ToString());

            _caseDataHelper.UpdateCase(primaryKey, fieldData);
        }

        [When(@"I create the case")]
        public void WhenICallTheMethodToCreateTheCase()
        {
            var caseModel = _scenarioContext.Get<CaseModel>(ScenarioContextType.CaseModel.ToString());

            _caseDataHelper.CreateCase(caseModel);
        }

        [When(@"I retrieve the case")]
        public void WhenICallTheMethodToRetrieveTheCase()
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextType.PrimaryKey.ToString());
            var caseModel = _caseDataHelper.GetCase(primaryKey);

            _scenarioContext.Set(caseModel, ScenarioContextType.CaseModel.ToString());
        }

        [When(@"I retrieve a list of cases")]
        public void WhenIGetAListOfCases()
        {
            var cases = _caseDataHelper.GetCases();

            _scenarioContext.Set(cases, ScenarioContextType.CaseModels.ToString());
        }


        [Then(@"the case is successfully created")]
        public void ThenTheCaseIsSuccessfullyCreated()
        {
            var caseModel = _scenarioContext.Get<CaseModel>(ScenarioContextType.CaseModel.ToString());
            var exists = _caseDataHelper.CaseExists(caseModel.PrimaryKey);
            
            Assert.IsTrue(exists);
        }

        [Then(@"the correct case is returned")]
        [Then(@"the case is updated successfully")]
        public void ThenTheCaseIsUpdatedSuccessfully(Dictionary<string, string> fieldData)
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextType.PrimaryKey.ToString());
            var existingCase = _caseDataHelper.GetCase(primaryKey);

            Assert.AreEqual(primaryKey, existingCase.PrimaryKey);

            foreach (var field in fieldData)
            {
                Assert.AreEqual(field.Value, existingCase.FieldData[field.Key]);
            }
        }

        [Then(@"all existing cases are returned")]
        public void ThenAllExistingCasesAreReturned(IEnumerable<string> primaryKeys)
        {
            var cases = _scenarioContext.Get<List<CaseModel>>(ScenarioContextType.CaseModels.ToString());

            foreach (var caseModel in primaryKeys.Select(primaryKey => cases.FirstOrDefault(c => c.PrimaryKey == primaryKey)))
            {
                Assert.IsNotNull(caseModel);
            }
        }

        [When(@"I check to see if the case exists")]
        public void WhenICallTheMethodToCheckIfTheCaseExists()
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextType.PrimaryKey.ToString());

            var exists = _caseDataHelper.CaseExists(primaryKey);

            _scenarioContext.Set(exists, ScenarioContextType.Exists.ToString());
        }

        [Then(@"the case no longer exists in blaise")]
        public void ThenTheCaseNoLongerExistsInBlaise()
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextType.PrimaryKey.ToString());

            var exists = _caseDataHelper.CaseExists(primaryKey);

            Assert.IsFalse(exists);
        }


        [Then(@"the case exists")]
        public void ThenTrueIsReturned()
        {
            var exists = _scenarioContext.Get<bool>(ScenarioContextType.Exists.ToString());

            Assert.IsTrue(exists);
        }

        [Then(@"the case does not exist")]
        public void ThenFalseIsReturned()
        {
            var exists = _scenarioContext.Get<bool>(ScenarioContextType.Exists.ToString());

            Assert.IsFalse(exists);
        }

        [When(@"I delete the case")]
        public void WhenIDeleteTheCase()
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextType.PrimaryKey.ToString());

            _caseDataHelper.DeleteCase(primaryKey);
        }

        [AfterScenario]
        public void CleanUpDataAfter()
        {
            CleanUpData();
        }

        private void CleanUpData()
        {
            _caseDataHelper.DeleteCasesInDatabase();
        }
    }
}
