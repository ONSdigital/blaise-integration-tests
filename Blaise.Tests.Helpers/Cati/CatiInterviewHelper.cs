using System.Collections.Generic;
using System.Threading;
using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.User;
using Blaise.Tests.Models.User;

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
            var interviewLoginPage = new InterviewLoginPage();
            interviewLoginPage.LoadPage();
            interviewLoginPage.LogIntoInterviewPortal(CatiConfigurationHelper.CatiInterviewUsername, CatiConfigurationHelper.CatiInterviewPassword);
        }

        public void ClickPlayButtonToAccessCase()
        {
            var caseInfoPage = new CaseInfoPage();
            caseInfoPage.LoadPage();
            Thread.Sleep(5000);
            caseInfoPage.ClickPlayButton();
        }

        public void CreateInterviewUser()
        {
            var interviewUser = new UserModel
            {
                UserName = CatiConfigurationHelper.CatiInterviewUsername,
                Password = CatiConfigurationHelper.CatiInterviewPassword,
                Role = CatiConfigurationHelper.InterviewRole,
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName
            };
            UserHelper.GetInstance().CreateUser(interviewUser);
        }

        public void AddSurveyFilter()
        {
            var dayBatchPage = new DayBatchPage();
            dayBatchPage.AddFilters();
        }

        public void SetupDayBatchTimeParameters()
        {
            Thread.Sleep(3000);
            var daybatchPage = new DayBatchPage();
            daybatchPage.ModifyDayBatchEntry();
        }

        public string GetCaseIdText()
        {
            var interviewPage = new InterviewPage();
            return interviewPage.GetCaseIdText();
        }

        public void DeleteInterviewUser()
        {
            UserHelper.GetInstance().RemoveUser(CatiConfigurationHelper.CatiInterviewUsername);
        }

        public void LoginButtonIsAvailable()
        {
            var interviewPage = new InterviewLoginPage();
            interviewPage.LoginButtonIsAvailable();
        }
    }
}