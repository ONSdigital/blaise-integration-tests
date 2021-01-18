using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SurveyPage : BasePage
    {
        private const string ClearCatiDataButtonPath = @"//*[@id='MVCGridTable_SurveysGrid']/tbody/tr/td[9]/a";
        private const string BackupDataButtonId = "chkBackupAll";
        private const string ClearDataButtonId = "chkClearAll";
        private const string ExecuteButtonPath = "//input[@value='Execute']";

        public SurveyPage() : base(CatiConfigurationHelper.SurveyUrl)
        {
        }

        public void ClearDayBatchEntries()
        {
            ClickButtonByXPath(ClearCatiDataButtonPath);
            ClickButtonById(BackupDataButtonId);
            ClickButtonById(ClearDataButtonId);
            ClickButtonByXPath(ExecuteButtonPath);
        }
    }
}
