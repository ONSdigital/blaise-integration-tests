namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using System.Linq;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class DeleteConfirmationPage : BasePage
    {
        private const string ContinueButtonId = "confirm-delete";
        private const string QuestionnaireTableId = "questionnaire-table";
        private const string DeletedSummaryHeadingPath = "//div[contains(@class,'ons-panel')][.//h1[contains(normalize-space(),'deleted successfully')]]//h1";

        public DeleteConfirmationPage()
            : base(DqsConfigurationHelper.ConfirmDeleteUrl)
        {
        }

        public void ClickContinueButton()
        {
            ClickButtonById(ContinueButtonId);
        }

        public void WaitForDeletionToComplete()
        {
            BrowserHelper
                .Wait("Timed out waiting for questionnaire list to reload after deletion")
                .Until(driver =>
                    driver.FindElements(By.Id(QuestionnaireTableId)).Any() ||
                    driver.FindElements(By.XPath(DeletedSummaryHeadingPath)).Any());
        }

        public void WaitForPageToLoad()
        {
            ButtonIsAvailableById(ContinueButtonId);
        }

        protected override By PageIdentityBy => By.Id(ContinueButtonId);
    }
}
