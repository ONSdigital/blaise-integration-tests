using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Blaise.Tests.Helpers.Cati
{
    public class CatiHelper
    {
        private readonly IWebDriver _driver;
        private static CatiHelper _currentInstance;

        public CatiHelper()
        {
            _driver = new ChromeDriver(CatiConfigurationHelper.ChromeDriver);
        }

        public static CatiHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new CatiHelper());
        }

        public void LogIntoCati()
        {
            LoginPage loginPage = new LoginPage(_driver);
            loginPage.GoToPage();
            loginPage.CatiLogin(CatiConfigurationHelper.CatiUsername, CatiConfigurationHelper.CatiPassword);
        }

        public void CreateDayBatch()
        {
            SetSurveyDays();

            DayBatchPage dayPage = new DayBatchPage(_driver);
            dayPage.GoToPage();
            dayPage.CreateDayBatch();
        }

        public string GetDaybatchEntriesText()
        {
            LogIntoCati();

            DayBatchPage dayBatchPage = new DayBatchPage(_driver);
            dayBatchPage.GoToPage();
            return dayBatchPage.GetDaybatchEntriesText();
        }

        public void LoadCaseInformation()
        {
            LogIntoCati();
            var caseInformationPage = new CaseInformationPage(_driver);
            caseInformationPage.GoToPage();

            caseInformationPage.LoadCase();
        }

        public string AccessCase()
        {
            var interviewPage = new InterviewPage(_driver);
            interviewPage.GoToPage();
            return interviewPage.GetSaveAndContinueButton();
        }

        private void SetSurveyDays()
        {
            SpecificationPage specPage = new SpecificationPage(_driver);
            specPage.GoToPage();
            specPage.SetSurveyDay();
        }

    }
}
