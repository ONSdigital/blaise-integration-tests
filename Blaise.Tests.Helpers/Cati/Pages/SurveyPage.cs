using System;
using System.Configuration;
using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SurveyPage : BasePage
    {
        private readonly string _blaiseVersion =
            ConfigurationManager.AppSettings["ENV_BLAISE_VERSION"];

        private const string FilterButtonPath = "//*[contains(text(), 'Filters')]";
        private const string ApplyButtonPath = "//*[contains(text(), 'Apply')]";
        private readonly string _surveyRadioButton =
            $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        public SurveyPage()
            : base(CatiConfigurationHelper.SurveyUrl)
        {
        }

        #region Versioned selectors

        private string ClearCatiDataButtonPath
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "//*[@id='MVCGridTable_SurveysGrid']/tbody/tr/td[9]/a";

                    case "v16":
                        return "//*[contains(text(), 'Clear')]";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        private string BackupDataButtonSelector
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "chkBackupAll"; // ID

                    case "v16":
                        return "qa_backupdata_0"; // ID

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        private string ClearDataButtonSelector
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "chkClearAll"; // ID

                    case "v16":
                        return "//label[@for='qa_clear_all']"; // XPath

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        private string ExecuteButtonPath
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "//input[@value='Execute']";

                    case "v16":
                        return "//button[@id='qa_btn_submit']";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        #endregion

        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            return _blaiseVersion?.ToLowerInvariant() switch
            {
                "v14" => BodyContainsText("Showing"),
                "v16" => BodyContainsText("Surveys"),
                _ => throw new ConfigurationErrorsException(
                    $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.")
            };
        }

        public void ClearDayBatchEntries()
        {
            Thread.Sleep(2000);

            ClickButtonByXPath(ClearCatiDataButtonPath);

            // Backup checkbox
            ClickButtonById(BackupDataButtonSelector);

            // Clear checkbox differs by selector type
            if (_blaiseVersion?.ToLowerInvariant() == "v14")
            {
                ClickButtonById(ClearDataButtonSelector);
            }
            else
            {
                ClickButtonByXPath(ClearDataButtonSelector);
            }

            ClickButtonByXPath(ExecuteButtonPath);
        }

        public void ApplyFilter()
        {
            ClickButtonByXPath(FilterButtonPath);

            var filterButtonText = GetElementTextByPath(FilterButtonPath);
            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(_surveyRadioButton);
                ClickButtonByXPath(ApplyButtonPath);
            }
        }
    }
}
