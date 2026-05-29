namespace Blaise.Tests.Helpers.Dqs.Pages
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class UploadPage : BasePage
    {
        private const string FileSelectorId = "survey-selector";
        private const string ContinueButtonId = "continue-deploy-button";
        private const string NoRadioButtonId = "no";
        private const string YesRadioButtonId = "yes";
        private const string QuestionnaireExistsHeadingPath = "//h1[contains(normalize-space(),'already exists')]";
        private const string ConfirmOverwriteHeadingPath = "//h1[contains(normalize-space(),'overwrite questionnaire')]";
        private const string DeploymentOutcomeHeadingPath = "//h1[contains(normalize-space(),'Questionnaire') and (contains(normalize-space(),'deployed successfully') or contains(normalize-space(),'deploy failed'))]";
        private const string ToStartDateSummaryValuePath = "//div[contains(@class,'ons-summary__item')][.//div[normalize-space()='Telephone Operations start date']]//span[contains(@class,'ons-summary__text')]";
        private const string TmReleaseDateHeadingPath = "//h1[contains(normalize-space(),'Totalmobile release date')]";
        private const string LiveDateTextBoxId = "set-date";
        private const string ToStartDateFieldName = "toStartDate";
        private const string TmReleaseDateFieldName = "tmReleaseDate";
        private const string CancelButtonId = "cancel-deploy-button";

        public UploadPage()
            : base(DqsConfigurationHelper.UploadUrl)
        {
        }

        public void SelectFileToUpload(string questionnairePath)
        {
            PopulateInputById(FileSelectorId, questionnairePath);
        }

        public void SelectContinueButton()
        {
            ClickButtonById(ContinueButtonId);
        }

        public void WaitForUploadCompletion()
        {
            BrowserHelper
                .Wait("Timed out waiting for deployment outcome")
                .Until(driver => driver.FindElements(By.XPath(DeploymentOutcomeHeadingPath)).Any());
        }

        public void WaitForQuestionnaireAlreadyExistsPage()
        {
            BrowserHelper
                .Wait("Timed out waiting for questionnaire already exists step")
                .Until(driver => driver.FindElements(By.XPath(QuestionnaireExistsHeadingPath)).Any());
        }

        public void SelectNoToStartDateButton()
        {
            ClickButtonById(NoRadioButtonId);
        }

        public void SelectYesLiveDateButton()
        {
            ClickButtonById(YesRadioButtonId);
        }

        public string GetToStartDateSummaryText()
        {
            return GetElementTextByPath(ToStartDateSummaryValuePath);
        }

        public void SelectContinueOverwriteButton()
        {
            ClickButtonById(ContinueButtonId);
        }

        public void SelectCancelButton()
        {
            ClickButtonById(CancelButtonId);
        }

        internal void SetLiveDate(string date)
        {
            var input = BrowserHelper
                .Wait("Timed out waiting for date input")
                .Until(FindDateInput);

            TrySetDateValue(input, date);

            BrowserHelper
                .Wait($"Timed out waiting for date value '{date}'")
                .Until(driver =>
                {
                    var current = FindDateInput(driver);
                    var value = current?.GetAttribute("value") ?? string.Empty;
                    return DateValueMatches(value, date);
                });
        }

        public void SkipTmReleaseDateIfPresent()
        {
            if (!IsTmReleaseDateStepVisible())
            {
                return;
            }

            ClickButtonById(NoRadioButtonId);
            ClickButtonById(ContinueButtonId);
        }

        private bool IsTmReleaseDateStepVisible()
        {
            return BrowserHelper.ElementExistsByXPath(TmReleaseDateHeadingPath, TimeSpan.FromSeconds(2));
        }

        private IWebElement FindDateInput(IWebDriver driver)
        {
            var byId = driver.FindElements(By.Id(LiveDateTextBoxId))
                .FirstOrDefault(candidate => candidate.Displayed);
            if (byId != null)
            {
                return byId;
            }

            var byToStartDateName = driver.FindElements(By.Name(ToStartDateFieldName))
                .FirstOrDefault(candidate => candidate.Displayed);
            if (byToStartDateName != null)
            {
                return byToStartDateName;
            }

            return driver.FindElements(By.Name(TmReleaseDateFieldName))
                .FirstOrDefault(candidate => candidate.Displayed);
        }

        private void TrySetDateValue(IWebElement element, string date)
        {
            try
            {
                element.Clear();
                element.SendKeys(date);
            }
            catch (ElementNotInteractableException)
            {
                SetDateValueByScript(element, date);
                return;
            }
            catch (InvalidElementStateException)
            {
                SetDateValueByScript(element, date);
                return;
            }

            var value = element.GetAttribute("value") ?? string.Empty;
            if (!DateValueMatches(value, date))
            {
                SetDateValueByScript(element, date);
            }
        }

        private static bool DateValueMatches(string value, string expectedDate)
        {
            if (value.IndexOf(expectedDate, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            if (DateTime.TryParse(expectedDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                var iso = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                var display = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                return value.IndexOf(iso, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    value.IndexOf(display, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }

        private static void SetDateValueByScript(IWebElement element, string date)
        {
            BrowserHelper.ExecuteJavaScript(
                "arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('input', { bubbles: true })); arguments[0].dispatchEvent(new Event('change', { bubbles: true }));",
                element,
                date);
        }

        public void WaitForConfirmOverwritePage()
        {
            BrowserHelper
                .Wait("Timed out waiting for overwrite confirmation step")
                .Until(driver => driver.FindElements(By.XPath(ConfirmOverwriteHeadingPath)).Any());
        }

        protected override By PageIdentityBy => By.Id(FileSelectorId);
    }
}
