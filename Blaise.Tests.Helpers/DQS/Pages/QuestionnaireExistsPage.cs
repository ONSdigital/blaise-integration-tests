using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class QuestionnaireExistsPage : BasePage
    {
        private readonly string overwriteId = "confirm-overwrite";
        private readonly string cancelId = "cancel-keep";
        private readonly string saveId = "confirm-save";

        public QuestionnaireExistsPage() : base(DqsConfigurationHelper.QuestionnaireExistsUrl)
        {
        }
        
        public void SelectOverwrite()
        {
            ClickButtonById(overwriteId);
        }

        public void SelectCancel()
        {
            ClickButtonById(cancelId);
        }

        public void SelectSaveButton()
        {
            ClickButtonById(saveId);
        }
    }
}
