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

        public void ClickPlayButtonToAccessCase()
        {
            var caseInfoPage = new CaseInfoPage();
            caseInfoPage.LoadPage();
            caseInfoPage.ApplyFilters();
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
            dayBatchPage.LoadPage();
            dayBatchPage.ApplyFilters();
        }

        public void SetupDayBatchTimeParameters()
        {
            var daybatchPage = new DayBatchPage();
            //Makes me sad but Blaise refreshes the table dom object after the page has initialised 
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

        public void WaitForCaseToLoad(string caseId)
        {
            // Define the maximum time to wait for the case to load
            var maxWaitTime = TimeSpan.FromSeconds(30);
            var startTime = DateTime.Now;

            // Wait for the case to load by checking the HTML of the current window
            while (!BrowserHelper.CurrentWindowHTML().Contains(caseId))
            {
                // If the maximum wait time is exceeded, throw an exception
                if (DateTime.Now - startTime > maxWaitTime)
                {
                    throw new TimeoutException($"Timed out waiting for case {caseId} to load.");
                }

                // Wait for a short time before checking again
                Thread.Sleep(500);
            }
        }
    }
}