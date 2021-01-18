using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class QuestionnaireExistsPage : BasePage
    {
        private readonly string overwriteId = "confirm-overwrite";
        private readonly string cancelId = "cancel-keep";

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
    }
}
