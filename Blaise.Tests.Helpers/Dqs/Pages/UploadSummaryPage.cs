namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class UploadSummaryPage : BasePage
    {
        private const string SummaryHeadingPath = "//div[contains(@class,'ons-panel')][.//h1[contains(normalize-space(),'deployed successfully') or contains(normalize-space(),'deploy failed')]]//h1";

        public UploadSummaryPage()
            : base(DqsConfigurationHelper.UploadSummaryUrl)
        {
        }

        public string GetUploadSummaryText()
        {
            return GetElementTextByPath(SummaryHeadingPath);
        }

        protected override By PageIdentityBy => By.XPath(SummaryHeadingPath);
    }
}
