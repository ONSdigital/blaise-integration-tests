using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
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
            LoginPage loginPage = new LoginPage();
            loginPage.LoadPage();
            loginPage.LoginToCati(CatiConfigurationHelper.CatiAdminUsername, CatiConfigurationHelper.CatiAdminPassword);
        }

        public void CreateDayBatch()
        {
            SetSurveyDays();

            DayBatchPage dayPage = new DayBatchPage();
            dayPage.LoadPage();
            dayPage.CreateDayBatch();
        }

        public string GetDaybatchEntriesText()
        {
            LogIntoCatiManagementPortal();

            DayBatchPage dayBatchPage = new DayBatchPage();
            dayBatchPage.LoadPage();
            return dayBatchPage.GetDaybatchEntriesText();
        }
        
        private void SetSurveyDays()
        {
            SpecificationPage specPage = new SpecificationPage();
            specPage.LoadPage();
            specPage.SetSurveyDay();
        }

        public void ClearDayBatchEnteries()
        {
            SurveyPage surveyPage = new SurveyPage();
            surveyPage.LoadPage();
            surveyPage.ClearDayBatchEnteries();
        }
    }
}
