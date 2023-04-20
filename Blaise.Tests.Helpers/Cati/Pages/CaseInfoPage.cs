using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class CaseInfoPage : BasePage
    {
        private string CaseIDCell = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[2]";
        private string PlayButton = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a/span";
        private const string FilterButton = "//*[contains(text(), 'Filters')]";
        private string SurveyRadioButton = $"//*[normalize-space()='{BlaiseConfigurationHelper.InstrumentName}']";
        private string ApplyButton = "//*[contains(text(), 'Apply')]";

        public CaseInfoPage() : base(CatiConfigurationHelper.CaseInfoUrl)
        {
        }

        public void ClickPlayButton()
        {
            ClickButtonByXPath(PlayButton);
        }

        public void ApplyFilters()
        {
            ClickButtonByXPath(FilterButton);
            var filterButtonText = GetElementTextByPath(FilterButton);
            if (filterButtonText != "Filters (active)")
            {
                ClickButtonByXPath(SurveyRadioButton);
                ClickButtonByXPath(ApplyButton);
            }
            ClickButtonByXPath(FilterButton);
        }

        public void WaitUntilFirstCaseIs(string caseId)
        {
            WaitUntilElementByXPathContainsText(CaseIDCell, caseId);
        }
    }
}
