using System.Threading;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class CaseInformationPage : BasePage
    {
        private readonly string _loadCaseButtonPath = @"//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a";

        public CaseInformationPage(IWebDriver driver) : base(driver, CatiConfigurationHelper.CaseInfoUrl)
        {
        }

        public void LoadCase()
        {
            ClickButtonByXPath(_loadCaseButtonPath);
        }
    }
}
