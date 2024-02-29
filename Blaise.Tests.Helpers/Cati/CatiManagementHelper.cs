using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Tobi;
using System;
using System.Threading;
using Blaise.Tests.Helpers.User;
using Blaise.Tests.Models.User;
using System.Collections.Generic;

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

        public void CreateAdminUser()
        {
            Console.WriteLine($"CatiManagementHelper: Creating admin user '{CatiConfigurationHelper.CatiAdminUsername}'");
            var adminUser = new UserModel
            {
                UserName = CatiConfigurationHelper.CatiAdminUsername,
                Password = CatiConfigurationHelper.CatiAdminPassword,
                Role = CatiConfigurationHelper.AdminRole,
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName
            };
            UserHelper.GetInstance().CreateUser(adminUser);
            Console.WriteLine($"CatiManagementHelper: Created admin user '{CatiConfigurationHelper.CatiAdminUsername}'");
        }

        public void LogIntoCatiManagementPortal()
        {
            var loginPage = new LoginPage();
            loginPage.LoadPage();
            loginPage.LoginToCati(CatiConfigurationHelper.CatiAdminUsername, CatiConfigurationHelper.CatiAdminPassword);
        }

        public void LogIntoCatiManagementPortalAsAnInterviewer()
        {
            var loginPage = new LoginPage();
            loginPage.LoadPage();
            loginPage.LoginToCati(CatiConfigurationHelper.CatiInterviewUsername, CatiConfigurationHelper.CatiInterviewPassword);
        }

        public void CreateDayBatch()
        {
            Console.WriteLine($"CatiManagementHelper: CreateDayBatch: Create survey days for Questionnaire {BlaiseConfigurationHelper.InstrumentName}");
            SetSurveyDays();

            Console.WriteLine($"CatiManagementHelper: CreateDayBatch: Create daybatch for Questionnaire {BlaiseConfigurationHelper.InstrumentName}");
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
