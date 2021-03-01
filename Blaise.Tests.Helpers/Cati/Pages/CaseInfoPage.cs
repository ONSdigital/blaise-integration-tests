using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class CaseInfoPage : BasePage
    {
        private string PlayButton = "//*[@id='MVCGridTable_CaseInfoGrid']/tbody/tr[1]/td[19]/a/span";

        public CaseInfoPage() : base(CatiConfigurationHelper.CaseInfoUrl)
        {
        }

        public void ClickPlayButton()
        {
            ClickButtonByXPath(PlayButton);

        }

    }
}
