using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Blaise.Tests.Helpers.Cati
{
    public class CatiManagementHelper
    {
        private readonly IWebDriver _driver;
        private static CatiManagementHelper _currentInstance;

        public CatiManagementHelper()
        {
            _driver = new ChromeDriver(CatiConfigurationHelper.ChromeDriver);
        }

        public static CatiManagementHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new CatiManagementHelper());
        }

        public string CurrentUrl()
        {
            return _driver.Url;
        }

        public void LogIntoCatiManagementPortal()
        {
            LoginPage loginPage = new LoginPage(_driver);
            loginPage.LoadPage();
            loginPage.LoginToCati(CatiConfigurationHelper.CatiAdminUsername, CatiConfigurationHelper.CatiAdminPassword);
        }

        public void CreateDayBatch()
        {
            SetSurveyDays();

            DayBatchPage dayPage = new DayBatchPage(_driver);
            dayPage.LoadPage();
            dayPage.CreateDayBatch();
        }

        public string GetDaybatchEntriesText()
        {
            LogIntoCatiManagementPortal();

            DayBatchPage dayBatchPage = new DayBatchPage(_driver);
            dayBatchPage.LoadPage();
            return dayBatchPage.GetDaybatchEntriesText();
        }
        
        private void SetSurveyDays()
        {
            SpecificationPage specPage = new SpecificationPage(_driver);
            specPage.LoadPage();
            specPage.SetSurveyDay();
        }
    }
}
