using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class QuestionnaireExistsPage : BasePage
    {
        private const string OverwriteId = "confirm-overwrite";
        private const string CancelId = "cancel-keep";
        private const string SaveId = "confirm-save";

        public QuestionnaireExistsPage() : base(DqsConfigurationHelper.QuestionnaireExistsUrl)
        {
        }
        
        public void SelectOverwrite()
        {
            ClickButtonById(OverwriteId);
        }

        public void SelectCancel()
        {
            ClickButtonById(CancelId);
        }

        public void SelectSaveButton()
        {
            ClickButtonById(SaveId);
        }
    }
}
