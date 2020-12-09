using Blaise.Tests.Helpers.RestApi;
using TechTalk.SpecFlow;

namespace Blaise.RestApi.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class QuestionnaireSteps
    {
        [When(@"the API is queried to return all active questionnaires")]
        public void WhenTheApiIsQueriedToReturnAllActiveQuestionnaires()
        {
           var n = RestApiHelper.GetAllActiveSurveys<Questionnaire>("Questionnaires");
        }

    }
}
