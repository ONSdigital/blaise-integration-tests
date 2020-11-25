using Blaise.Smoke.Tests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blaise.Smoke.Tests.Helpers
{
    public class SeleniumHelper
    {
        public IWebDriver driver;
        private ConfigurationHelper _configurationHelper;

        public SeleniumHelper()
        {
            this._configurationHelper = new ConfigurationHelper();

            driver = new ChromeDriver(_configurationHelper.ChromeDriver);
        }

        private void CatiLogin()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.GoToPage();
            loginPage.CatiLogin(_configurationHelper.CatiUsername, _configurationHelper.CatiPassword);
        }

        private void SetSurveyDays()
        {
            SpecificationPage specPage = new SpecificationPage(driver);
            specPage.GoToPage();
            specPage.SetSurveyDay();
        }

        public void CreateDayBatch()
        {
            CatiLogin();

            SetSurveyDays();

            DayBatchPage dayPage = new DayBatchPage(driver);
            dayPage.GoToPage();
            dayPage.CreateDayBatch();
        }

        public string CheckDayBatchEnteries()
        {
            CatiLogin();

            DayBatchPage dayBatchPage = new DayBatchPage(driver);
            dayBatchPage.GoToPage();
            return dayBatchPage.CheckEnteriesInDayBatch();
        }

        public void LoadCaseInformation()
        {
            CatiLogin();
            CaseInfoPage caseInfoPage = new CaseInfoPage(driver);
            caseInfoPage.GoToPage();
            
            caseInfoPage.LoadCase();
        }

        public string AccessCase()
        {
            InterviewPage interviewPage = new InterviewPage(driver);
            interviewPage.GoToPage();
            return interviewPage.GetSaveAndContinueButton();
        }


    }
}
