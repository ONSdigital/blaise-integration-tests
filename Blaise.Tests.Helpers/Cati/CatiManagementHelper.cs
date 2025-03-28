﻿using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Tobi;
using Blaise.Tests.Helpers.User;
using Blaise.Tests.Models.User;
using System;
using System.Collections.Generic;
using System.Threading;

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
            var adminUser = new UserModel
            {
                Username = CatiConfigurationHelper.CatiAdminUsername,
                Password = CatiConfigurationHelper.CatiAdminPassword,
                Role = CatiConfigurationHelper.AdminRole,
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName
            };
            UserHelper.GetInstance().CreateUser(adminUser);
        }

        public void LogIntoCatiDashboardAsAdministrator()
        {
            var loginPage = new LoginPage();
            loginPage.LoadPage();
            loginPage.LoginToCati(CatiConfigurationHelper.CatiAdminUsername, CatiConfigurationHelper.CatiAdminPassword);
            Thread.Sleep(2000);
        }

        public void LogIntoCatiDashboardAsInterviewer()
        {
            var loginPage = new LoginPage();
            loginPage.LoadPage();
            loginPage.LoginToCati(CatiConfigurationHelper.CatiInterviewUsername, CatiConfigurationHelper.CatiInterviewPassword);
            Thread.Sleep(2000);
        }

        public void CreateDayBatch()
        {
            SetSurveyDays();

            DayBatchHelper.GetInstance().CreateDayBatch(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
        }

        public string GetDaybatchEntriesText()
        {
            LogIntoCatiDashboardAsAdministrator();

            var dayBatchPage = new DayBatchPage();
            dayBatchPage.LoadPage();
            dayBatchPage.ApplyFilters();
            Thread.Sleep(2000);
            return dayBatchPage.GetDaybatchEntriesText();
        }

        public void SetSurveyDays()
        {
            DayBatchHelper.GetInstance().SetSurveyDay(BlaiseConfigurationHelper.QuestionnaireName, DateTime.Today);
        }

        public void DeleteAdminUser()
        {
            UserHelper.GetInstance().RemoveUser(CatiConfigurationHelper.CatiAdminUsername);
        }

        public void ClearDayBatchEntries()
        {
            var surveyPage = new SurveyPage();
            surveyPage.LoadPage();
            surveyPage.ApplyFilter();

            surveyPage.ClearDayBatchEntries();
        }
    }
}
