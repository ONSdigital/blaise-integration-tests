using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Browser;

namespace Blaise.Tests.Helpers.Cati
{
    public class CatiManagementHelper
    {
        private static CatiManagementHelper _currentInstance;
        
        public static CatiManagementHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new CatiManagementHelper());
        }

        public string CurrentUrl()
        {
            return BrowserHelper.CurrentUrl;
        }

        public void LogIntoCatiManagementPortal()
        {
            var loginPage = new LoginPage();
            loginPage.LoadPage();
            loginPage.LoginToCati(CatiConfigurationHelper.CatiAdminUsername, CatiConfigurationHelper.CatiAdminPassword);
        }

        public void CreateDayBatch(string instrumentName)
        {
            SetSurveyDays(instrumentName);

            var dayPage = new DayBatchPage();
            dayPage.LoadPage();
            dayPage.CreateDayBatch();
        }

        public string GetDaybatchEntriesText()
        {
            LogIntoCatiManagementPortal();

            var dayBatchPage = new DayBatchPage();
            dayBatchPage.LoadPage();
            return dayBatchPage.GetDaybatchEntriesText();
        }
        
        private void SetSurveyDays(string instrumentName)
        {
            var specPage = new SpecificationPage();
            specPage.LoadPage();
            
            specPage.SetSurveyDay(instrumentName);
        }

        public void ClearDayBatchEntries()
        {
            var surveyPage = new SurveyPage();
            surveyPage.LoadPage();
            surveyPage.ClearDayBatchEntries();
        }
    }
}
