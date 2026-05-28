namespace Blaise.Tests.Helpers.Cati
{
    using System;
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Cati.Pages;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Tobi;
    using Blaise.Tests.Helpers.User;
    using Blaise.Tests.Models.User;

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
            var adminUser = new UserModel
            {
                Username = CatiConfigurationHelper.CatiAdminUsername,
                Password = CatiConfigurationHelper.CatiAdminPassword,
                Role = CatiConfigurationHelper.AdminRole,
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName,
            };
            UserHelper.GetInstance().CreateUser(adminUser);
        }

        public void LogIntoCatiDashboardAsAdministrator()
        {
            Console.WriteLine("Navigating to login page.");
            var loginPage = new LoginPage();
            loginPage.LoadLoginPage();
            Console.WriteLine("Logging in as administrator.");
            var initialUrl = BrowserHelper.CurrentUrl;
            loginPage.LoginToCati(CatiConfigurationHelper.CatiAdminUsername, CatiConfigurationHelper.CatiAdminPassword);
            BrowserHelper.WaitForUrlToChange(initialUrl, 30);
            Console.WriteLine($"Login process completed. Current URL: {BrowserHelper.CurrentUrl}");
        }

        public void LogIntoCatiDashboardAsInterviewer()
        {
            var loginPage = new LoginPage();
            loginPage.LoadLoginPage();
            var initialUrl = BrowserHelper.CurrentUrl;
            loginPage.LoginToCati(CatiConfigurationHelper.CatiInterviewUsername, CatiConfigurationHelper.CatiInterviewPassword);
            BrowserHelper.WaitForUrlToChange(initialUrl, 30);
        }

        public void CreateDaybatch()
        {
            Console.WriteLine("Setting survey days.");
            SetSurveyDays();
            Console.WriteLine("Creating daybatch.");
            DaybatchHelper.GetInstance().CreateDaybatch(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
            Console.WriteLine("Daybatch created successfully.");
        }

        public string GetDaybatchEntriesText()
        {
            LogIntoCatiDashboardAsAdministrator();

            var daybatchPage = new DaybatchPage();
            daybatchPage.LoadPage();
            daybatchPage.ApplyFilter();
            daybatchPage.WaitForDaybatchTable();
            return daybatchPage.GetDaybatchEntriesText();
        }

        public void SetSurveyDays()
        {
            DaybatchHelper.GetInstance().SetSurveyDay(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
        }

        public void DeleteAdminUser()
        {
            UserHelper.GetInstance().RemoveUser(CatiConfigurationHelper.CatiAdminUsername);
        }

        public void ClearDaybatchEntries()
        {
            Console.WriteLine("Loading survey page to clear daybatch entries.");
            var surveyPage = new SurveyPage();
            surveyPage.LoadPage();
            Console.WriteLine("Applying filter on survey page.");
            surveyPage.ApplyFilter();
            surveyPage.WaitForSurveyTable();
            Console.WriteLine("Clearing daybatch entries.");
            surveyPage.ClearDaybatchEntries();
            Console.WriteLine("Daybatch entries cleared successfully.");
        }
    }
}
