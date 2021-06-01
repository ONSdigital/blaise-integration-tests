using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class ConfirmOverwritePage : BasePage
    {
        private const string ConfirmOverwriteButtonId = "confirm-overwrite";
        private const string ContinueOverwriteButtonId = "confirm-continue";

        public ConfirmOverwritePage() : base(DqsConfigurationHelper.ConfirmOverwriteUrl)
        {
        }

        public void ClickConfirmOverwriteButton()
        {
            ClickButtonById(ConfirmOverwriteButtonId);
        }

        public void ClickContinueButton()
        {
            ClickButtonById(ContinueOverwriteButtonId);
        }
}
}
