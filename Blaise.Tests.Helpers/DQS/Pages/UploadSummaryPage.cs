using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.DQS.Pages
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
