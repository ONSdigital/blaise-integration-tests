using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.User;
using Blaise.Tests.Models.User;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Blaise.Tests.Helpers.Cati
{
    public class CatiInterviewHelper
    {
        private static CatiInterviewHelper _currentInstance;

        public static CatiInterviewHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new CatiInterviewHelper());
        }

        public void AccessInterviewPortal()
        {
            Console.WriteLine($"Attempting to log onto to interview portal with the user '{CatiConfigurationHelper.CatiInterviewUsername}'");
            var interviewLoginPage = new InterviewLoginPage();
            interviewLoginPage.LoadPage();
            interviewLoginPage.LogIntoInterviewPortal(CatiConfigurationHelper.CatiInterviewUsername, CatiConfigurationHelper.CatiInterviewPassword);
            Console.WriteLine($"Loged onto to interview portal with the user '{CatiConfigurationHelper.CatiInterviewUsername}'");
        }

        public void ClickPlayButtonToAccessCase(string caseId)
        {
            Console.WriteLine($"Attempting to access the case '{caseId}'");
            var caseInfoPage = new CaseInfoPage();
            caseInfoPage.RefreshPageUntilCaseIsPlayable(caseId);
            caseInfoPage.ClickPlayButton();
            Console.WriteLine($"Accessed to access the case '{caseId}'");
        }

        public void CreateAdminUser()
        {
            Console.WriteLine($"Creating admin user '{CatiConfigurationHelper.CatiAdminUsername}'");
            var adminUser = new UserModel
            {
                UserName = CatiConfigurationHelper.CatiAdminUsername,
                Password = CatiConfigurationHelper.CatiAdminPassword,
                Role = CatiConfigurationHelper.AdminRole,
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName
            };
            UserHelper.GetInstance().CreateUser(adminUser);
            Console.WriteLine($"Created admin user '{CatiConfigurationHelper.CatiAdminUsername}'");
        }

        public void CreateInterviewUser()
        {
            Console.WriteLine($"Creating interviewer user '{CatiConfigurationHelper.CatiInterviewUsername}'");
            var interviewUser = new UserModel
            {
                UserName = CatiConfigurationHelper.CatiInterviewUsername,
                Password = CatiConfigurationHelper.CatiInterviewPassword,
                Role = CatiConfigurationHelper.InterviewRole,
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName
            };
            UserHelper.GetInstance().CreateUser(interviewUser);
            Console.WriteLine($"Created interviewer user '{CatiConfigurationHelper.CatiInterviewUsername}'");
        }

        public void AddSurveyFilter()
        {
            Console.WriteLine("Attempting to filter survey");
            var dayBatchPage = new DayBatchPage();
            dayBatchPage.LoadPage();
            dayBatchPage.ApplyFilters();
            Console.WriteLine("Filtered survey");
        }

        public void SetupDayBatchTimeParameters()
        {
            var daybatchPage = new DayBatchPage();
            //makes me sad but Blaise refreshes the table dom object after the page has initialized 
            Thread.Sleep(5000);
            daybatchPage.ModifyDayBatchEntry();
        }

        public string GetCaseIdText()
        {
            var interviewPage = new InterviewPage();
            return interviewPage.GetCaseIdText();
        }

        public void WaitForFirstFocusObject()
        {
            var interviewPage = new InterviewPage();
            interviewPage.WaitForFirstFocusObject();
        }

        public void DeleteAdminUser()
        {
            Console.WriteLine($"Attempting to delete the admin user {CatiConfigurationHelper.CatiAdminUsername}");
            UserHelper.GetInstance().RemoveUser(CatiConfigurationHelper.CatiAdminUsername);
            Console.WriteLine($"Deleted the admin user {CatiConfigurationHelper.CatiAdminUsername}");
        }

        public void DeleteInterviewUser()
        {
            Console.WriteLine($"Attempting to delete the interviewer user {CatiConfigurationHelper.CatiAdminUsername}");
            UserHelper.GetInstance().RemoveUser(CatiConfigurationHelper.CatiInterviewUsername);
            Console.WriteLine($"Deletet the interviewer user {CatiConfigurationHelper.CatiAdminUsername}");
        }

        public void LoginButtonIsAvailable()
        {
            var interviewPage = new InterviewLoginPage();
            interviewPage.LoginButtonIsAvailable();
        }
    }
}
