using System.Collections.Generic;
using Blaise.Tests.Helpers.RestApi;
using Blaise.Tests.Models;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.RestApi.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class QuestionnaireSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private const string ApiResponse = "ApiResponse";

        public QuestionnaireSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When(@"the API is queried to return all active questionnaires")]
        public void WhenTheApiIsQueriedToReturnAllActiveQuestionnaires()
        {
           var listOfActiveQuestionnaires = RestApiHelper.GetAllActiveQuestionnaires();
           _scenarioContext.Set(listOfActiveQuestionnaires, ApiResponse);
        }

        [Then(@"details of questionnaire a is returned")]
        public void ThenDetailsOfQuestionnaireAIsReturned()
        {
            var listOfActiveQuestionnaires = _scenarioContext.Get<List<Questionnaire>>(ApiResponse);
            Assert.AreEqual(1, listOfActiveQuestionnaires.Count);
        }
    }
}
