using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.DQS.Pages
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
    }
}
