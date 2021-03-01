using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class UploadPage : BasePage
    {
        private readonly string fileSelectorId = "survey-selector";
        private readonly string continueButtonId = "continue-deploy-button";

        public UploadPage() : base(DqsConfigurationHelper.UploadUrl)
        {
        }

        public void SelectFileToUpload(string instrumentPath)
        {
            PopulateInputById(fileSelectorId, instrumentPath);
        }

        public void SelectContinueButton()
        {
            ClickButtonById(continueButtonId);
        }

        public void WaitForUploadCompletion()
        {
            WaitForPageToChange(DqsConfigurationHelper.UploadSummaryUrl);
        }

        public void WaitForQuestionnaireAlreadyExistsPage()
        {
            WaitForPageToChange(DqsConfigurationHelper.QuestionnaireExistsUrl);
        }
    }
}
