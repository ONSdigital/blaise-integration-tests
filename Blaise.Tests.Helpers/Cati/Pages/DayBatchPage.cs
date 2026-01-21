// ReSharper disable InconsistentNaming
namespace Blaise.Tests.Helpers.Cati.Pages
{
    using System;
    using System.Configuration;
    using System.Threading;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework;
    using OpenQA.Selenium;

    public class DayBatchPage : BasePage
    {
        private readonly string _blaiseVersion =
            ConfigurationManager.AppSettings["ENV_BLAISE_VERSION"];

        private const string _dayBatchCreateButtonId = "btnCreateDaybatch";
        private const string _createButtonPath = "//input[@value='Create']";
        private const string _questionnaireDropDownId = "InstrumentId";

        private const string _filterButton = "//*[contains(text(), 'Filters')]";
        private readonly string _surveyRadioButton =
            $"//*[normalize-space()='{BlaiseConfigurationHelper.QuestionnaireName}']";

        private const string _applyButton = "//*[contains(text(), 'Apply')]";

        private string DayBatchEntryPath
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return
                            $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']";

                    case "v16":
                        return
                            $"//table[@id='Daybatch_content_table']//td[contains(., '{BlaiseConfigurationHelper.QuestionnaireName}')]";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        private string ModifyEntrySelector
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return
                            $"//table[@id='MVCGridTable_DaybatchGrid']//td[preceding-sibling::td='{BlaiseConfigurationHelper.QuestionnaireName}']/a";

                    case "v16":
                        return "qa_editrecord_0";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        private bool ModifyEntryIsById =>
            _blaiseVersion?.Equals("v16", StringComparison.OrdinalIgnoreCase) == true;

        private string StartTimeId =>
            _blaiseVersion?.Equals("v16", StringComparison.OrdinalIgnoreCase) == true
                ? "qa_starttime"
                : "NewStartTimeAmPm";

        private string EndTimeId =>
            _blaiseVersion?.Equals("v16", StringComparison.OrdinalIgnoreCase) == true
                ? "qa_endtime"
                : "NewEndTimeAmPm";

        private string UpdateButtonSelector
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "//input[@value='Update']";

                    case "v16":
                        return "qa_btn_submit";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        private bool UpdateButtonIsById =>
            _blaiseVersion?.Equals("v16", StringComparison.OrdinalIgnoreCase) == true;

        public DayBatchPage()
            : base(CatiConfigurationHelper.DayBatchUrl)
        {
        }

        public void CreateDayBatch()
        {
            ClickButtonById(_dayBatchCreateButtonId);
            Thread.Sleep(2000);

            SelectDropDownValueById(
                _questionnaireDropDownId,
                BlaiseConfigurationHelper.QuestionnaireName);

            Thread.Sleep(3000);
            ClickButtonByXPath(_createButtonPath);
        }

        public string GetDaybatchEntriesText()
        {
            return GetElementTextByPath(DayBatchEntryPath);
        }

        internal void ModifyDayBatchEntry()
        {
            if (ModifyEntryIsById)
            {
                ClickButtonById(ModifyEntrySelector);
            }
            else
            {
                ClickButtonByXPath(ModifyEntrySelector);
            }

            PopulateInputById(StartTimeId, "12:00 AM");
            PopulateInputById(EndTimeId, "11:59 PM");

            if (UpdateButtonIsById)
            {
                ClickButtonById(UpdateButtonSelector);
            }
            else
            {
                ClickButtonByXPath(UpdateButtonSelector);
            }
        }

        public void ApplyFilters()
        {
            Thread.Sleep(5000);
            ClickButtonByXPath(_filterButton);

            var filterButtonText = GetElementTextByPath(_filterButton);
            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(_surveyRadioButton);
                ClickButtonByXPath(_applyButton);
            }
        }
    }
}
