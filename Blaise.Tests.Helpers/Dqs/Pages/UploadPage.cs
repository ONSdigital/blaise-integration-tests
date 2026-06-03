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

        protected override By PageIdentityBy => By.Id(FileSelectorId);

        public void SelectFileToUpload(string questionnairePath)
        {
            PopulateInputById(FileSelectorId, questionnairePath);
        }

        public void SelectContinueButton()
        {
            TryClickContinueButton();
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

        public void SkipTmReleaseDateIfPresent()
        {
            if (!IsTmReleaseDateStepVisible())
            {
                return;
            }

            ClickButtonById(NoRadioButtonId);
            ClickButtonById(ContinueButtonId);
        }

        public void WaitForConfirmOverwritePage()
        {
            BrowserHelper
                .Wait("Timed out waiting for overwrite confirmation step")
                .Until(driver => driver.FindElements(By.XPath(ConfirmOverwriteHeadingPath)).Any());
        }

        internal void SetLiveDate(string date)
        {
            var input = BrowserHelper
                .Wait("Timed out waiting for date input")
                .Until(FindDateInput);

            var parsed = TryParseDate(date, out var parsedDate, out var isoDate, out var displayDate);
            TrySetDateValue(input, parsed ? (DateTime?)parsedDate : null, isoDate, displayDate);

            BrowserHelper
                .Wait($"Timed out waiting for date value '{date}'")
                .Until(driver =>
                {
                    var current = FindDateInput(driver);
                    var value = current?.GetAttribute("value") ?? string.Empty;
                    return DateValueMatches(value, isoDate) || DateValueMatches(value, displayDate);
                });
        }

        private static bool TrySendKeys(IWebElement element, string value)
        {
            try
            {
                element.Click();
                element.Clear();
                element.SendKeys(value);
                return true;
            }
            catch (ElementNotInteractableException)
            {
                return false;
            }
            catch (InvalidElementStateException)
            {
                return false;
            }
        }

        private static bool DateValueMatches(string value, string expectedDate)
        {
            if (string.IsNullOrEmpty(expectedDate))
            {
                return false;
            }

            if (value.IndexOf(expectedDate, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            if (TryParseDate(expectedDate, out var date, out var iso, out var display))
            {
                return value.IndexOf(iso, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    value.IndexOf(display, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }

        private static bool TryParseDate(
            string date,
            out DateTime parsedDate,
            out string isoDate,
            out string displayDate)
        {
            var formats = new[]
            {
                "yyyy-MM-dd",
                "dd/MM/yyyy",
                "d/M/yyyy",
                "yyyy/M/d",
            };

            if (DateTime.TryParseExact(
                date,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedDate))
            {
                isoDate = parsedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                displayDate = parsedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                return true;
            }

            parsedDate = default;
            isoDate = date;
            displayDate = date;
            return false;
        }

        private static void SetDateValueByScript(IWebElement element, string date, DateTime? parsedDate)
        {
            if (parsedDate.HasValue)
            {
                BrowserHelper.ExecuteJavaScript(
                    "var el = arguments[0]; var value = arguments[1]; var year = arguments[2]; var month = arguments[3]; var day = arguments[4]; if (el.type === 'date') { el.valueAsDate = new Date(year, month - 1, day); } el.value = value; el.dispatchEvent(new Event('input', { bubbles: true })); el.dispatchEvent(new Event('change', { bubbles: true }));",
                    element,
                    date,
                    parsedDate.Value.Year,
                    parsedDate.Value.Month,
                    parsedDate.Value.Day);
                return;
            }

            BrowserHelper.ExecuteJavaScript(
                "arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('input', { bubbles: true })); arguments[0].dispatchEvent(new Event('change', { bubbles: true }));",
                element,
                date);
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

        private void TryClickContinueButton()
        {
            try
            {
                BrowserHelper.ScrollIntoViewAndClickByIdWithRetry(ContinueButtonId);
            }
            catch (WebDriverException)
            {
                BrowserHelper.ClickWithJavaScript(By.Id(ContinueButtonId));
            }
        }

        private void TrySetDateValue(
            IWebElement element,
            DateTime? parsedDate,
            string isoDate,
            string displayDate)
        {
            var primaryValue = string.IsNullOrEmpty(isoDate) ? displayDate : isoDate;

            if (!TrySendKeys(element, primaryValue))
            {
                SetDateValueByScript(element, primaryValue, parsedDate);
            }

            var value = element.GetAttribute("value") ?? string.Empty;
            if (!DateValueMatches(value, primaryValue) && !DateValueMatches(value, displayDate))
            {
                if (!TrySendKeys(element, displayDate))
                {
                    SetDateValueByScript(element, displayDate, parsedDate);
                }
            }
        }
    }
}
