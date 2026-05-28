namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class DeleteConfirmationPage : BasePage
    {
        private const string ContinueButtonId = "confirm-delete";

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
            WaitForPageToChange(DqsConfigurationHelper.DqsUrl);
        }

        public void WaitForPageToLoad()
        {
            WaitForPageToChange(DqsConfigurationHelper.ConfirmDeleteUrl);
        }

        protected override By PageIdentityBy => By.Id(ContinueButtonId);
    }
}
