using System;
using System.Configuration;
using System.Threading;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class CaseInfoPage : BasePage
    {
        private readonly string _blaiseVersion =
            ConfigurationManager.AppSettings["ENV_BLAISE_VERSION"];

        private string QuestionnaireCellPath
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[1]";

                    case "v16":
                        return "//*[@id='CaseInfo_content_table']/tbody/tr[1]/td[1]";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        private string CaseIdCellPath
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";

                    case "v16":
                        return "//*[@id='CaseInfo_content_table']/tbody/tr[1]/td[2]";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        private string PlayButtonSelector
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a/span";

                    case "v16":
                        return "qa_startcase_0";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        private bool PlayButtonIsById =>
            _blaiseVersion?.Equals("v16", StringComparison.OrdinalIgnoreCase) == true;

        private const string _filterButton = "//*[contains(text(), 'Filters')]";
        private readonly string _surveyRadioButton =
            $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        private const string _applyButton = "//*[contains(text(), 'Apply')]";

        public CaseInfoPage()
            : base(CatiConfigurationHelper.CaseInfoUrl)
        {
        }

        public void RefreshPageUntilCaseIsPlayable(string caseId)
        {
            var attempts = 0;
            do
            {
                LoadPage();
                ApplyFilters();
                WaitUntilFirstCaseQuestionnaireIs(BlaiseConfigurationHelper.QuestionnaireName);
                WaitUntilFirstCaseIs(caseId);

                attempts++;
                if (attempts > 5)
                {
                    throw new Exception("Giving up after 5 attempts waiting for play button");
                }
            }
            while (!FirstCaseIsPlayable());
        }

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            switch (_blaiseVersion?.ToLowerInvariant())
            {
                case "v14":
                    return BodyContainsText("Showing");

                case "v16":
                    return BodyContainsText("Case Info");

                default:
                    throw new ConfigurationErrorsException(
                        $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
            }
        }


        public void ClickPlayButton()
        {
            var numberOfWindows = BrowserHelper.GetNumberOfWindows();
            var attempts = 0;

            while (BrowserHelper.GetNumberOfWindows() == numberOfWindows)
            {
                if (PlayButtonIsById)
                {
                    ClickButtonById(PlayButtonSelector);
                }
                else
                {
                    ClickButtonByXPath(PlayButtonSelector);
                }

                Thread.Sleep(250);
                attempts++;

                if (attempts > 5)
                {
                    throw new Exception("Timed out waiting for new window to open.");
                }
            }
        }

        public void ApplyFilters()
        {
            ClickButtonByXPath(_filterButton);
            var filterButtonText = GetElementTextByPath(_filterButton);

            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(_surveyRadioButton);
                ClickButtonByXPath(_applyButton);
            }

            ClickButtonByXPath(_filterButton);
        }

        private void WaitUntilFirstCaseQuestionnaireIs(string questionnaire)
        {
            WaitUntilElementByXPathContainsText(QuestionnaireCellPath, questionnaire);
        }

        private void WaitUntilFirstCaseIs(string caseId)
        {
            WaitUntilElementByXPathContainsText(CaseIdCellPath, caseId);
        }

        public bool FirstCaseIsPlayable()
        {
            return PlayButtonIsById
                ? ElementIsDisplayed(By.Id(PlayButtonSelector))
                : ElementIsDisplayed(By.XPath(PlayButtonSelector));
        }
    }
}
