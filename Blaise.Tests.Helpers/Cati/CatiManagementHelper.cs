using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Tobi;
using System;
using System.Threading;
using NUnit.Framework;

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
            try
            {
                var loginPage = new LoginPage();
                loginPage.LoadPage();
                loginPage.LoginToCati(CatiConfigurationHelper.CatiAdminUsername, CatiConfigurationHelper.CatiAdminPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LogIntoCatiManagementPortal: {ex.Message}, inner exception: {{ex.InnerException?.Message}}\"");
                Assert.Fail($"The test failed to complete - {ex.Message}, inner exception: {ex.InnerException?.Message}");
            }

        }

        public void LogIntoCatiManagementPortalAsAnInterviewer()
        {
            try
            {
                var loginPage = new LoginPage();
                loginPage.LoadPage();
                loginPage.LoginToCati(CatiConfigurationHelper.CatiInterviewUsername, CatiConfigurationHelper.CatiInterviewPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LogIntoCatiManagementPortalAsAnInterviewer: {ex.Message}, inner exception: {{ex.InnerException?.Message}}\"");
                Assert.Fail($"The test failed to complete - {ex.Message}, inner exception: {ex.InnerException?.Message}");
            }
        }

        public void CreateDayBatch()
        {
            Console.WriteLine($"CatiManagementHelper CreateDayBatch: Create survey days for Questionnaire {BlaiseConfigurationHelper.InstrumentName}");
            SetSurveyDays();

            Console.WriteLine($"CatiManagementHelper CreateDayBatch: Create daybatch for Questionnaire {BlaiseConfigurationHelper.InstrumentName}");
            DayBatchHelper.GetInstance().CreateDayBatch(BlaiseConfigurationHelper.InstrumentName, DateTime.Today);
        }

        public string GetDaybatchEntriesText()
        {
            LogIntoCatiManagementPortal();

            var dayBatchPage = new DayBatchPage();
            dayBatchPage.LoadPage();
            dayBatchPage.ApplyFilters();
            Thread.Sleep(2000);
            return dayBatchPage.GetDaybatchEntriesText();
        }

        public void SetSurveyDays()
        {
            DayBatchHelper.GetInstance().SetSurveyDay(BlaiseConfigurationHelper.InstrumentName, DateTime.Today);
        }

        public void ClearDayBatchEntries()
        {
            Console.WriteLine($"CatiManagementHelper ClearDayBatchEntries: Clearing daybatch entries for Questionnaire {BlaiseConfigurationHelper.InstrumentName}");
            var surveyPage = new SurveyPage();

            Console.WriteLine("CatiManagementHelper ClearDayBatchEntries: LoadPage");
            surveyPage.LoadPage();

            Console.WriteLine("CatiManagementHelper ClearDayBatchEntries: ApplyFilter");
            surveyPage.ApplyFilter();

            Console.WriteLine("CatiManagementHelper ClearDayBatchEntries: ClearDayBatchEntries");
            surveyPage.ClearDayBatchEntries();
        }
    }
}
