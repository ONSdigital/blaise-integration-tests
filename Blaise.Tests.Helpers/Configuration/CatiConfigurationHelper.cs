namespace Blaise.Tests.Helpers.Configuration
{
    using System;
    using Blaise.Tests.Helpers.Framework.Extensions;

    public static class CatiConfigurationHelper
    {
        private static readonly Guid _adminPassword;
        private static readonly Guid _interviewerPassword;

        static CatiConfigurationHelper()
        {
            _adminPassword = Guid.NewGuid();
            _interviewerPassword = Guid.NewGuid();
        }

        public static string CatiAdminUsername => "DSTAdminUser";
        public static string CatiAdminPassword => $"{_adminPassword}";
        public static string AdminRole => "DST";
        public static string CatiInterviewUsername => "DSTTestUser";
        public static string CatiInterviewPassword => $"{_interviewerPassword}";
        public static string InterviewRole => "DST";

        public static string CatiBaseUrl
        {
            get
            {
                string baseUrl = ConfigurationExtensions.GetVariable("ENV_BLAISE_CATI_URL");
                if (!baseUrl.StartsWith("http://") && !baseUrl.StartsWith("https://"))
                {
                    baseUrl = "https://" + baseUrl;
                }
                return baseUrl;
            }
        }

        public static string LoginUrl => $"{CatiBaseUrl}/blaise/account/login";
        public static string DaybatchUrl => $"{CatiBaseUrl}/blaise/daybatch";
        public static string SchedulerUrl => $"{CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}";
        public static string SpecificationUrl => $"{CatiBaseUrl}/blaise/specification";
        public static string SurveyUrl => $"{CatiBaseUrl}/blaise";
        public static string CaseUrl => $"{CatiBaseUrl}/Blaise/CaseInfo/StartSurvey?url={CatiBaseUrl}/{BlaiseConfigurationHelper.QuestionnaireName}/&rp.KeyValue=";
        public static string CaseInfoUrl => $"{CatiBaseUrl}/blaise/CaseInfo";
        public static string NewDashboardCaseInfoUrl => $"{CatiBaseUrl}/BlaiseDashboard/Cati/CaseInfo";
    }
}

namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Threading;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class CaseInfoPage : BasePage
    {
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private const string ApplyButton = "//*[contains(text(), 'Apply')]";
        private readonly string _surveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        private static bool? _isNewDashboard;

        private bool UseNewSelectors
        {
            get
            {
                if (!_isNewDashboard.HasValue)
                {
                    _isNewDashboard = BrowserHelper.ElementExistsByXPath("//i[contains(@class, 'bi-bell-fill')]", TimeSpan.FromSeconds(2));
                }
                return _isNewDashboard.Value;
            }
        }

        public void NavigateToVersionSpecificPage()
        {
            if (UseNewSelectors)
            {
                BrowserHelper.NavigateToPage(CatiConfigurationHelper.NewDashboardCaseInfoUrl);
            }
            else
            {
                BrowserHelper.NavigateToPage(CatiConfigurationHelper.CaseInfoUrl);
            }
        }

        private string QuestionnaireCellPath => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']//tr//td[@aria-colindex='1']"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[1]";

        private string CaseIdCellPath => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']//tr//td[@aria-colindex='2']"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";

        private string PlayButtonSelector => UseNewSelectors
            ? "//*[@id='CaseInfo_content_table']/tbody/tr[1]/td[19]/div/div/div"
            : "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a/span";

        public CaseInfoPage()
            : base(CatiConfigurationHelper.CaseInfoUrl)
        {
        }

        public void RefreshPageUntilCaseIsPlayable(string caseId)
        {
            var attempts = 0;
            do
            {
                NavigateToVersionSpecificPage();
                ApplyFilter();

                if (UseNewSelectors)
                {
                    BrowserHelper.WaitUntilGridHasLoadedData();
                }

                WaitUntilFirstCaseQuestionnaireIs(BlaiseConfigurationHelper.QuestionnaireName);
                WaitUntilFirstCaseIs(caseId);

                attempts++;
                if (attempts > 5)
                {
                    throw new Exception($"Giving up after 5 attempts waiting for play button for case {caseId}");
                }
            }
            while (!FirstCaseIsPlayable());
        }

        public void ClickPlayButton()
        {
            var numberOfWindows = BrowserHelper.GetNumberOfWindows();
            var attempts = 0;

            while (BrowserHelper.GetNumberOfWindows() == numberOfWindows)
            {
                if (UseNewSelectors)
                {
                    BrowserHelper.ScrollIntoViewAndClick(By.XPath(PlayButtonSelector));
                }
                else
                {
                    BrowserHelper.ClickByXPathWithJavaScriptWithRetry(PlayButtonSelector);
                }

                Thread.Sleep(250);
                attempts++;

                if (attempts > 5)
                {
                    throw new Exception("Timed out waiting for new window to open.");
                }
            }
        }

        public void ApplyFilter()
        {
            if (UseNewSelectors)
            {
                ClickButtonByXPath("//div[@e-mappinguid='qa_instrumentid' and contains(@class, 'e-filtermenudiv')]");
                var dropdownSelector = "//span[contains(@class, 'e-ddl') and .//input[@id='qa_instrumentnameidfilter']]";
                ClickButtonByXPath(dropdownSelector);
                var listOptionPath = $"//li[contains(@class, 'e-list-item') and text()='{BlaiseConfigurationHelper.QuestionnaireName}']";
                ClickButtonByXPath(listOptionPath);
                ClickButtonByXPath("//button[contains(@class, 'e-flmenu-okbtn') and text()='Filter']");
                Thread.Sleep(1000);
            }
            else
            {
                ClickButtonByXPath(FilterButton);
                var filterButtonText = GetElementTextByPath(FilterButton);
                if (filterButtonText != "Filters (active)")
                {
                    ClickButtonByXPath(_surveyRadioButton);
                    ClickButtonByXPath(ApplyButton);
                }
                ClickButtonByXPath(FilterButton);
            }
        }

        public bool FirstCaseIsPlayable()
        {
            return ElementIsDisplayed(By.XPath(PlayButtonSelector));
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return UseNewSelectors
                ? BodyDoesNotContainText("No records to display")
                : BodyContainsText("Showing");
        }

        private void WaitUntilFirstCaseQuestionnaireIs(string questionnaire)
        {
            WaitUntilElementByXPathContainsText(QuestionnaireCellPath, questionnaire);
        }

        private void WaitUntilFirstCaseIs(string caseId)
        {
            WaitUntilElementByXPathContainsText(CaseIdCellPath, caseId);
        }
    }
}
