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
            PopulateInputById(_fileSelectorId, questionnairePath);
        }

        public void SelectContinueButton()
        {
            ClickButtonById(_continueButtonId);
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
            ClickButtonById(_noRadioButtonId);
        }

        public void SelectYesLiveDateButton()
        {
            ClickButtonById(_yesRadioButtonId);
        }

        public string GetToStartDateSummaryText()
        {
            return GetElementTextByPath(_liveDateTextPath);
        }

        internal void SetLiveDate(string date)
        {
            PopulateInputById(_liveDateTextBoxId, date);
        }

        public void SelectContinueOverwriteButton()
        {
            ClickButtonById(_continueOverwriteRadioButtonId);
        }

        public void SelectCancelButton()
        {
            ClickButtonById(_cancelButtonId);
        }
    }
}
