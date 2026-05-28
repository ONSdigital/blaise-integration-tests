namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class CannotOverwritePage : BasePage
    {
        private const string CannotOverwriteDivPath = "//div[contains(@class, 'error ons-panel')]";

        public CannotOverwritePage()
            : base(DqsConfigurationHelper.CannotOverwriteUrl)
        {
        }

        public string GetUploadSummaryText()
        {
            return GetElementTextByPath(CannotOverwriteDivPath);
        }

        protected override By PageIdentityBy => By.XPath(CannotOverwriteDivPath);
    }
}
