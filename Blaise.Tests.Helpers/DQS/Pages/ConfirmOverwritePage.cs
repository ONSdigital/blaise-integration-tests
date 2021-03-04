using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class ConfirmOverwritePage : BasePage
    {
        private readonly string confirmOverwriteButtonId = "confirm-overwrite";
        private readonly string continueOverwriteButtonId = "confirm-continue";

        public ConfirmOverwritePage() : base(DqsConfigurationHelper.ConfirmOverwriteUrl)
        {
        }

        public void ClickConfirmOverwriteButton()
        {
            ClickButtonById(confirmOverwriteButtonId);
        }

        public void ClickContinueButton()
        {
            ClickButtonById(continueOverwriteButtonId);
        }
}
}
