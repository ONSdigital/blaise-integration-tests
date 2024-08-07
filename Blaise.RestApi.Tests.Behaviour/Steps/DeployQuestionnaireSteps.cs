using System;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Cloud;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.RestApi;
using Blaise.Tests.Helpers.Questionnaire;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.RestApi.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class DeployQuestionnaireSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly QuestionnaireHelper _questionnaireHelper;

        public DeployQuestionnaireSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _questionnaireHelper = QuestionnaireHelper.GetInstance();
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
            try
            {
                await RestApiHelper.GetInstance().DeployQuestionnaire(
                    RestApiConfigurationHelper.QuestionnaireUrl,
                    BlaiseConfigurationHelper.QuestionnaireBucketPath,
                    BlaiseConfigurationHelper.QuestionnairePackage);

                // Check if the questionnaire is erroneous after deployment
                _questionnaireHelper.CheckIfQuestionnaireIsErroneous(BlaiseConfigurationHelper.QuestionnaireName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deploying questionnaire: {ex.Message}");
                Assert.Fail($"Failed to deploy questionnaire: {ex.Message}");
            }
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