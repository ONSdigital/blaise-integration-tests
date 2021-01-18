using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class UploadSummaryPage : BasePage
    {
        private readonly string summaryDivPath = "//div[contains(@class, 'success panel')]";

        public UploadSummaryPage() : base(DqsConfigurationHelper.UploadSummaryUrl)
        {
        }

        public string GetUploadSummaryText()
        {
            return GetElementTextByPath(summaryDivPath);
        }
    }
}
