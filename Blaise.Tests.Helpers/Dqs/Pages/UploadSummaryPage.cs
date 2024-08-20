using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class UploadSummaryPage : BasePage
    {
        private const string SummaryDivPath = "//div[contains(@class, 'success ons-panel')]";

        public UploadSummaryPage() : base(DqsConfigurationHelper.UploadSummaryUrl)
        {
        }

        public string GetUploadSummaryText()
        {
            return GetElementTextByPath(SummaryDivPath);
        }
    }
}
