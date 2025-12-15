namespace Blaise.Tests.Behaviour.Steps
{
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Questionnaire;
    using NUnit.Framework;
    using Reqnroll;

    [Binding]
    public sealed class InstallQuestionnaireSteps
    {
        [Given(@"I have a questionnaire I want to install")]
        public void GivenIHaveAQuestionnaireIWantToInstall()
        {
            var questionnairePackage = BlaiseConfigurationHelper.QuestionnairePackage;

            if (string.IsNullOrWhiteSpace(questionnairePackage))
            {
                Assert.Fail("No questionnaire package has been configured");
            }
        }

        [Then(@"the questionnaire is available")]
        public void ThenTheQuestionnaireIsAvailable()
        {
            var questionnaireHasInstalled = QuestionnaireHelper.GetInstance().CheckQuestionnaireInstalled(
                BlaiseConfigurationHelper.QuestionnaireName,
                BlaiseConfigurationHelper.ServerParkName,
                60);

            Assert.That(
                questionnaireHasInstalled,
                Is.True,
                $"Questionnaire {BlaiseConfigurationHelper.QuestionnaireName} should be installed and active on server park {BlaiseConfigurationHelper.ServerParkName}");
        }
    }
}
