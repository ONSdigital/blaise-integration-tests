namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;

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
