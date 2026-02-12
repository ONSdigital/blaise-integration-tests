namespace Blaise.Tests.Helpers.Cati
{
    using System.Collections.Generic;
    using System.Threading;
    using Blaise.Tests.Helpers.Cati.Pages;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.User;
    using Blaise.Tests.Models.User;

    public class CatiInterviewHelper
    {
        private static CatiInterviewHelper _currentInstance;

        public static CatiInterviewHelper GetInstance()
        {
            return _currentInstance ?? (_currentInstance = new CatiInterviewHelper());
        }

        public void AccessCatiScheduler()
        {
            var schedulerPage = new SchedulerPage();
            schedulerPage.LoadPage();
            schedulerPage.LogIntoScheduler(CatiConfigurationHelper.CatiInterviewUsername, CatiConfigurationHelper.CatiInterviewPassword);
        }

        public void ClickPlayButtonToAccessCase(string caseId)
        {
            var caseInfoPage = new CaseInfoPage();
            caseInfoPage.RefreshPageUntilCaseIsPlayable(caseId);
            caseInfoPage.ClickPlayButton();
        }

        public void CreateInterviewUser()
        {
            var interviewUser = new UserModel
            {
                Username = CatiConfigurationHelper.CatiInterviewUsername,
                Password = CatiConfigurationHelper.CatiInterviewPassword,
                Role = CatiConfigurationHelper.InterviewRole,
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName,
            };
            UserHelper.GetInstance().CreateUser(interviewUser);
        }

        public void AddSurveyFilter()
        {
            var daybatchPage = new DaybatchPage();
            daybatchPage.LoadPage();
            daybatchPage.ApplyFilter();
        }

        public void SetupDaybatchTimeParameters()
        {
            var daybatchPage = new DaybatchPage();

            // Blaise refreshes the table dom object after the page has initialised
            Thread.Sleep(5000);
            daybatchPage.ModifyDaybatchEntry();
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
            var schedulerPage = new SchedulerPage();
            schedulerPage.LoginButtonIsAvailable();
        }
    }
}
