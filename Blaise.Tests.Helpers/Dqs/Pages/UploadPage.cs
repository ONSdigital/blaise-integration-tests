using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class UploadPage : BasePage
    {
        private const string _fileSelectorId = "survey-selector";
        private const string _continueButtonId = "continue-deploy-button";
        private const string _noRadioButtonId = "no";
        private const string _yesRadioButtonId = "yes";
        private const string _continueOverwriteRadioButtonId = "continue";
        private const string _liveDateTextPath = "//*[@id=\"formID\"]/div[1]/div/table/tbody[5]/tr/td[2]";
        private const string _liveDateTextBoxId = "set-live-date";
        private const string _cancelButtonId = "cancel-deploy-button";

        public UploadPage()
            : base(DqsConfigurationHelper.UploadUrl)
        {
        }

        public void SelectFileToUpload(string questionnairePath)
        {
            PopulateInputById(FileSelectorId, questionnairePath);
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

        public void SelectNoToStartDateButton()
        {
            ClickButtonById(NoRadioButtonId);
        }

        public void SelectYesLiveDateButton()
        {
            ClickButtonById(YesRadioButtonId);
        }

        public string GetToStartDateSummaryText()
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
