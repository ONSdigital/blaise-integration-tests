using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SurveyPage : BasePage
    {
        private readonly string _clearCatiDataButtonPath = @"//*[@id='MVCGridTable_SurveysGrid']/tbody/tr/td[9]/a";
        private readonly string _backupDataButtonId = "chkBackupAll";
        private readonly string _clearDataButtonId = "chkClearAll";
        private readonly string _executeButtonPath = "//input[@value='Execute']";

        public SurveyPage() : base(CatiConfigurationHelper.SurveyUrl)
        {
        }

        public void ClearDayBatchEnteries()
        {
            ClickButtonByXPath(_clearCatiDataButtonPath);
            ClickButtonById(_backupDataButtonId);
            ClickButtonById(_clearDataButtonId);
            ClickButtonByXPath(_executeButtonPath);
        }
    }
}
