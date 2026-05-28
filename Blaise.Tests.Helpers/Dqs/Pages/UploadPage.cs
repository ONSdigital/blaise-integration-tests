namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using System;
    using System.Linq;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class UploadPage : BasePage
    {
        private const string FileSelectorId = "survey-selector";
        private const string ContinueButtonId = "continue-deploy-button";
        private const string NoRadioButtonId = "no";
        private const string YesRadioButtonId = "yes";
        private const string QuestionnaireExistsHeadingPath = "//h1[contains(normalize-space(),'already exists')]";
        private const string ConfirmOverwriteHeadingPath = "//h1[contains(normalize-space(),'overwrite questionnaire')]";
        private const string DeploymentOutcomeHeadingPath = "//h1[contains(normalize-space(),'Questionnaire') and (contains(normalize-space(),'deployed successfully') or contains(normalize-space(),'deploy failed'))]";
        private const string ToStartDateSummaryValuePath = "//div[contains(@class,'ons-summary__item')][.//div[normalize-space()='Telephone Operations start date']]//span[contains(@class,'ons-summary__text')]";
        private const string TmReleaseDateHeadingPath = "//h1[contains(normalize-space(),'Totalmobile release date')]";
        private const string LiveDateTextBoxId = "set-date";
        private const string CancelButtonId = "cancel-deploy-button";

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
            BrowserHelper
                .Wait("Timed out waiting for deployment outcome")
                .Until(driver => driver.FindElements(By.XPath(DeploymentOutcomeHeadingPath)).Any());
        }

        public void WaitForQuestionnaireAlreadyExistsPage()
        {
            BrowserHelper
                .Wait("Timed out waiting for questionnaire already exists step")
                .Until(driver => driver.FindElements(By.XPath(QuestionnaireExistsHeadingPath)).Any());
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
            return GetElementTextByPath(ToStartDateSummaryValuePath);
        }

        public void SelectContinueOverwriteButton()
        {
            ClickButtonById(ContinueButtonId);
        }

        public void SelectCancelButton()
        {
            ClickButtonById(CancelButtonId);
        }

        internal void SetLiveDate(string date)
        {
            PopulateInputById(LiveDateTextBoxId, date);
            BrowserHelper.WaitForElementValue(By.Id(LiveDateTextBoxId), date, 10);
        }

        public void SkipTmReleaseDateIfPresent()
        {
            if (!IsTmReleaseDateStepVisible())
            {
                return;
            }

            ClickButtonById(NoRadioButtonId);
            ClickButtonById(ContinueButtonId);
        }

        private bool IsTmReleaseDateStepVisible()
        {
            return BrowserHelper.ElementExistsByXPath(TmReleaseDateHeadingPath, TimeSpan.FromSeconds(2));
        }

        public void WaitForConfirmOverwritePage()
        {
            BrowserHelper
                .Wait("Timed out waiting for overwrite confirmation step")
                .Until(driver => driver.FindElements(By.XPath(ConfirmOverwriteHeadingPath)).Any());
        }

        protected override By PageIdentityBy => By.Id(FileSelectorId);
    }
}
