using System.Threading.Tasks;
using Blaise.Tests.Helpers.Cloud;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.RestApi;
using TechTalk.SpecFlow;

namespace Blaise.RestApi.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DeployQuestionnaireSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public DeployQuestionnaireSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"there is a questionnaire available in a bucket")]
        public void GivenThereIsAnQuestionnaireAvailableInABucket()
        {
            CloudStorageHelper.GetInstance().UploadToBucket(
                BlaiseConfigurationHelper.QuestionnaireBucketPath,
                BlaiseConfigurationHelper.QuestionnairePackage);
        }

        [When(@"the API is called to deploy the questionnaire")]
        public async Task WhenTheApiIsCalledToDeployTheQuestionnaire()
        {
            await RestApiHelper.GetInstance().DeployQuestionnaire(
                RestApiConfigurationHelper.QuestionnaireUrl,
                BlaiseConfigurationHelper.QuestionnaireBucketPath,
                BlaiseConfigurationHelper.QuestionnairePackage);
        }


        [AfterScenario("deploy")]
        public void CleanUpScenario()
        {
            CloudStorageHelper.GetInstance().DeleteFromBucket(
                BlaiseConfigurationHelper.QuestionnaireBucketPath,
                BlaiseConfigurationHelper.QuestionnairePackage);
        }
    }
}
