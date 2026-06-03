namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class CannotOverwritePage : BasePage
    {
        private const string CannotOverwritePanelPath = "//div[contains(@class,'ons-panel')][.//p[contains(normalize-space(),'cannot overwrite a questionnaire')]]";

        public CannotOverwritePage()
            : base(DqsConfigurationHelper.CannotOverwriteUrl)
        {
        }

        protected override By PageIdentityBy => By.XPath(CannotOverwritePanelPath);

        public string GetUploadSummaryText()
        {
            return GetElementTextByPath(CannotOverwritePanelPath);
        }
    }
}
