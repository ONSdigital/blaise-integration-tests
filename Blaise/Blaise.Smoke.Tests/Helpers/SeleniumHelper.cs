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

        public SeleniumHelper()
        {
            driver = new ChromeDriver("");
        }

        private void CatiLogin()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.GoToPage();
            loginPage.CatiLogin("", "");
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
            return dayBatchPage.CheckEnteriesInDayBatch();
        }

        public void LoadCaseInformation()
        {
            CatiLogin();
            CaseInfoPage caseInfoPage = new CaseInfoPage(driver);
            caseInfoPage.GoToPage();
            
            caseInfoPage.loadCase();
        }

        public string AccessCase()
        {
            InterviewPage interviewPage = new InterviewPage(driver);
            interviewPage.GoToPage();
            return interviewPage.GetSaveAndContinueButton();
        }


    }
}
