using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class DeleteConfirmationPage : BasePage
    {
        private readonly string confirmDeleteRadioButtonId = "confirm-radio-delete";
        private readonly string continueButtonId = "confirm-delete";
        public DeleteConfirmationPage() : base(DqsConfigurationHelper.ConfirmDeleteUrl)
        {
        }

        public void ClickConfirmDeleteQuestionnaireButton()
        {
            ClickButtonById(confirmDeleteRadioButtonId);
        }

        public void ClickContinueButton()
        {
            ClickButtonById(continueButtonId);
        }

        public void WaitForDeletionToComplete()
        {
            WaitForPageToChange(DqsConfigurationHelper.DqsUrl);
        }
    }
}
