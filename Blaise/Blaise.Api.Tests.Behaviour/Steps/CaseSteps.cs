using System.Collections.Generic;
using System.Linq;
using Blaise.Api.Tests.Behaviour.Enums;
using Blaise.Api.Tests.Behaviour.Helpers;
using Blaise.Api.Tests.Behaviour.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Api.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class CaseSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;

        private readonly DataHelper _dataHelper;
        
        public CaseSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            _dataHelper = new DataHelper();
        }

        [BeforeScenario]
        public static void CleanUpDataBefore()
        {
            CleanUpData();
        }

        [Given(@"I have a new case I want to create")]
        public void GivenIHaveANewCaseIWantToCreate()
        {
            var caseModel = _dataHelper.BuildBasicCase();

            _scenarioContext.Set(caseModel, ScenarioContextTypes.CaseModel.ToString());
        }

        [Given(@"a case exists in blaise with the primary key '(.*)'")]
        public void GivenACaseExistsInBlaiseWithThePrimaryKey(string primaryKey)
        {
            var caseModel = _dataHelper.BuildCase(primaryKey);
            _dataHelper.CreateCase(caseModel);

            _scenarioContext.Set(primaryKey, ScenarioContextTypes.PrimaryKey.ToString());
        }

        [Given(@"a case does not exist in blaise with the primary key '(.*)'")]
        public void GivenACaseDoesNotExistInBlaiseWithThePrimaryKey(string primaryKey)
        {
            _scenarioContext.Set(primaryKey, ScenarioContextTypes.PrimaryKey.ToString());
        }
        
        [Given(@"a case exists in blaise with the following data")]
        public void GivenACaseExistsInBlaiseWithTheFollowingData(Dictionary<string, string> fieldData)
        {
            var caseModel = _dataHelper.BuildCase(fieldData);
            _dataHelper.CreateCase(caseModel);

            _scenarioContext.Set(caseModel.PrimaryKey, ScenarioContextTypes.PrimaryKey.ToString());
        }

        [Given(@"there are a number of existing cases in Blaise")]
        public void GivenThereAreANumberOfExistingCasesInBlaise(IEnumerable<string> primaryKeys)
        {
            foreach (var primaryKey in primaryKeys)
            {
                _dataHelper.CreateCase(_dataHelper.BuildCase(primaryKey));
            }
        }
        
        [When(@"I update the case with the following data")]
        public void WhenICallTheUpdateMethodForTheCase(Dictionary<string, string> fieldData)
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextTypes.PrimaryKey.ToString());

            _dataHelper.UpdateCase(primaryKey, fieldData);
        }

        [When(@"I create the case")]
        public void WhenICallTheMethodToCreateTheCase()
        {
            var caseModel = _scenarioContext.Get<CaseModel>(ScenarioContextTypes.CaseModel.ToString());

            _dataHelper.CreateCase(caseModel);
        }

        [When(@"I retrieve the case")]
        public void WhenICallTheMethodToRetrieveTheCase()
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextTypes.PrimaryKey.ToString());
            var caseModel = _dataHelper.GetCase(primaryKey);

            _scenarioContext.Set(caseModel, ScenarioContextTypes.CaseModel.ToString());
        }

        [When(@"I retrieve a list of cases")]
        public void WhenIGetAListOfCases()
        {
            var cases = _dataHelper.GetCases();

            _scenarioContext.Set(cases, ScenarioContextTypes.CaseModels.ToString());
        }


        [Then(@"the case is successfully created")]
        public void ThenTheCaseIsSuccessfullyCreated()
        {
            var caseModel = _scenarioContext.Get<CaseModel>(ScenarioContextTypes.CaseModel.ToString());
            var exists = _dataHelper.CaseExists(caseModel.PrimaryKey);
            
            Assert.IsTrue(exists);
        }

        [Then(@"the correct case is returned")]
        [Then(@"the case is updated successfully")]
        public void ThenTheCaseIsUpdatedSuccessfully(Dictionary<string, string> fieldData)
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextTypes.PrimaryKey.ToString());
            var existingCase = _dataHelper.GetCase(primaryKey);

            Assert.AreEqual(primaryKey, existingCase.PrimaryKey);

            foreach (var field in fieldData)
            {
                Assert.AreEqual(field.Value, existingCase.FieldData[field.Key]);
            }
        }

        [Then(@"all existing cases are returned")]
        public void ThenAllExistingCasesAreReturned(IEnumerable<string> primaryKeys)
        {
            var cases = _scenarioContext.Get<List<CaseModel>>(ScenarioContextTypes.CaseModels.ToString());

            foreach (var caseModel in primaryKeys.Select(primaryKey => cases.FirstOrDefault(c => c.PrimaryKey == primaryKey)))
            {
                Assert.IsNotNull(caseModel);
            }
        }

        [When(@"I check to see if the case exists")]
        public void WhenICallTheMethodToCheckIfTheCaseExists()
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextTypes.PrimaryKey.ToString());

            var exists = _dataHelper.CaseExists(primaryKey);

            _scenarioContext.Set(exists, ScenarioContextTypes.Exists.ToString());
        }

        [Then(@"the case no longer exists in blaise")]
        public void ThenTheCaseNoLongerExistsInBlaise()
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextTypes.PrimaryKey.ToString());

            var exists = _dataHelper.CaseExists(primaryKey);

            Assert.IsFalse(exists);
        }


        [Then(@"true is returned")]
        public void ThenTrueIsReturned()
        {
            var exists = _scenarioContext.Get<bool>(ScenarioContextTypes.Exists.ToString());

            Assert.IsTrue(exists);
        }

        [Then(@"false is returned")]
        public void ThenFalseIsReturned()
        {
            var exists = _scenarioContext.Get<bool>(ScenarioContextTypes.Exists.ToString());

            Assert.IsFalse(exists);
        }

        [When(@"I delete the case")]
        public void WhenIDeleteTheCase()
        {
            var primaryKey = _scenarioContext.Get<string>(ScenarioContextTypes.PrimaryKey.ToString());

            _dataHelper.DeleteCase(primaryKey);
        }

        [AfterScenario]
        public static void CleanUpDataAfter()
        {
            CleanUpData();
        }

        private static void CleanUpData()
        {
            var dataHelper = new DataHelper();
            dataHelper.DeleteCasesInDatabase();
        }
    }
}
