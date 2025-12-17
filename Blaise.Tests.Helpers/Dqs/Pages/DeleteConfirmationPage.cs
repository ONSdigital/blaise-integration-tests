namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;

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
    }
}
