using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class UploadPage : BasePage
    {
        private const string FileSelectorId = "survey-selector";
        private const string ContinueButtonId = "continue-deploy-button";

        public UploadPage() : base(DqsConfigurationHelper.UploadUrl)
        {
        }

        public void SelectFileToUpload(string instrumentPath)
        {
            PopulateInputById(FileSelectorId, instrumentPath);
        }

        public void SelectContinueButton()
        {
            ClickButtonById(ContinueButtonId);
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
