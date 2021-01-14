using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.DQS.Pages
{
    public class HomePage : BasePage
    {
        private readonly string deployQuestionnaireButtonID = "deploy-questionnaire-link";

        public HomePage() : base(DqsConfigurationHelper.DqsUrl)
        {
        }

        public void ClickDeployAQuestionnaire()
        {
            ClickButtonById(deployQuestionnaireButtonID);
        }
    }
}
