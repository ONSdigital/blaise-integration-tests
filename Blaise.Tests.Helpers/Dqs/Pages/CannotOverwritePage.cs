using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class CannotOverwritePage : BasePage
    {
        private const string _cannotOverwriteDivPath = "//div[contains(@class, 'error ons-panel')]";

        public CannotOverwritePage()
            : base(DqsConfigurationHelper.CannotOverwriteUrl)
        {
        }

        public string GetUploadSummaryText()
        {
            return GetElementTextByPath(_cannotOverwriteDivPath);
        }
    }
}
