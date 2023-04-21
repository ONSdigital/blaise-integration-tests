using Blaise.Tests.Helpers.Browser;
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
            var interviewLoginPage = new InterviewLoginPage();
            interviewLoginPage.LoadPage();
            interviewLoginPage.LogIntoInterviewPortal(CatiConfigurationHelper.CatiInterviewUsername, CatiConfigurationHelper.CatiInterviewPassword);
        }

        public void ClickPlayButtonToAccessCase(string caseId)
        {
            var caseInfoPage = new CaseInfoPage();

            var attempts = 0;
            do {
                caseInfoPage.LoadPage();
                caseInfoPage.ApplyFilters();
                caseInfoPage.WaitUntilFirstCaseQuestionnaireIs(BlaiseConfigurationHelper.InstrumentName);
                caseInfoPage.WaitUntilFirstCaseIs(caseId);

                attempts++;
                if (attempts > 5)
                {
                    throw new Exception("Giving up after 5 attempts waiting for play button");
                }
            } while (!caseInfoPage.FirstCaseIsPlayable());

            var numberOfWindows = BrowserHelper.GetNumberOfWindows();
            
            attempts = 0;
            while (BrowserHelper.GetNumberOfWindows() == numberOfWindows)
            {
                caseInfoPage.ClickPlayButton();
                Thread.Sleep(250);
                attempts++;
                if (attempts > 10)
                {
                    throw new Exception("Timed out waiting for new window to open.");
                }
            }
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
            dayBatchPage.LoadPage();
            dayBatchPage.ApplyFilters();
        }

        public void SetupDayBatchTimeParameters()
        {
            var daybatchPage = new DayBatchPage();
            //makes me sad but Blaise refreshes the table dom object after the page has initalised 
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
