using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class UploadPage : BasePage
    {
        private const string FileSelectorId = "survey-selector";
        private const string ContinueButtonId = "continue-deploy-button";
        private const string NoRadioButtonId = "no";
        private const string YesRadioButtonId = "yes";
        private const string ContinueOverwriteRadioButtonId = "continue";
        private const string LiveDateTextPath = "//*[@id=\"formID\"]/div[1]/div/table/tbody[5]/tr/td[2]";
        private const string LiveDateTextBoxId = "set-live-date";
        private const string CancelButtonId = "cancel-deploy-button";

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

        public void SelectNoLiveDateButton()
        {
            ClickButtonById(NoRadioButtonId);
        }

        public void SelectYesLiveDateButton()
        {
            ClickButtonById(YesRadioButtonId);
        }

        public string GetLiveDateSummaryText()
        {
            return GetElementTextByPath(LiveDateTextPath);
        }

        internal void SetLiveDate(string date)
        {
            PopulateInputById(LiveDateTextBoxId, date);
        }

        public void SelectContinueOverwriteButton()
        {
            ClickButtonById(ContinueOverwriteRadioButtonId);
        }

        public void SelectCancelButton()
        {
            ClickButtonById(CancelButtonId);
        }
    }
}
