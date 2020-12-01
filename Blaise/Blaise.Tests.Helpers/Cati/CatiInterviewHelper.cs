using System.Collections.Generic;
using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.User;
using Blaise.Tests.Models.User;
using OpenQA.Selenium.Chrome;

namespace Blaise.Tests.Helpers.Cati
{
    public class CatiInterviewHelper
    {
        private static CatiInterviewHelper _currentInstance;
        private ChromeDriver _driver;

        public static CatiInterviewHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new CatiInterviewHelper());
        }

        public CatiInterviewHelper()
        {
            _driver = new ChromeDriver(CatiConfigurationHelper.ChromeDriver);
        }

        public void LogIntoInterviewPortal()
        {
            var interviewLoginPage = new InterviewLoginPage(_driver);
            interviewLoginPage.LoadPage();
            interviewLoginPage.LogIntoInterviewPortal(CatiConfigurationHelper.CatiInterviewUsername, CatiConfigurationHelper.CatiInterviewPassword);
        }

        public void CreateInterviewUser()
        {
            var interviewUser = new UserModel
            {
                UserName = CatiConfigurationHelper.CatiInterviewUsername,
                Password = CatiConfigurationHelper.CatiInterviewPassword,
                Role = CatiConfigurationHelper.InterviewRole,
                ServerParks = new List<string>{BlaiseConfigurationHelper.ServerParkName},
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName
            };

            UserHelper.GetInstance().CreateUser(interviewUser);
        }

        public string GetCaseIdText()
        {
            var interviewPage = new InterviewPage(_driver);
            return interviewPage.GetCaseIdText();
        }

        public void DeleteInterviewUser()
        {
            UserHelper.GetInstance().RemoveUser(CatiConfigurationHelper.CatiInterviewUsername);
        }
    }
}