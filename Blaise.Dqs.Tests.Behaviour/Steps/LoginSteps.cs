using System;
using System.Collections.Generic;
using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.User;
using Blaise.Tests.Models.User;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class LoginSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private static readonly string UserName = $"BDSS-test-user-{Guid.NewGuid()}";
        private static readonly string Password = $"{Guid.NewGuid()}";

        [BeforeTestRun]
        public static void SetupTestRun()
        {
            var userModel = new UserModel
            {
                UserName = UserName,
                Password = Password,
                Role = "BDSS",
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName
            };

            UserHelper.GetInstance().CreateUser(userModel);
        }

        [Given(@"I am a BDSS user")]
        public void GivenIAmABdssUser()
        {
        }

        [Given(@"I have logged into to DQS")]
        public void GivenIHaveLoggedIntoToDqs()
        {
            LogInToDqs();
        }

        private void LogInToDqs()
        {
            try
            {
                DqsHelper.GetInstance().LogInToDqs(UserName, Password);
            }
            catch
            {
                TakeScreenShot("LogInError", "Error logging in");
            }
         
        }

        private static void LogOutOfDqs()
        {
            try
            {
                DqsHelper.GetInstance().LogOutOfToDqs();
            }
            catch
            {
                TakeScreenShot("LogOutError", "Error logging out");
            }

        }

        [AfterTestRun]
        public static void CleanUpTestRun()
        {
            UserHelper.GetInstance().RemoveUser(UserName);
        }
        
        private static void TakeScreenShot(string screenShotName, string screenShotDescription)
        {
            var screenShotFile = BrowserHelper.TakeScreenShot(TestContext.CurrentContext.WorkDirectory,
                screenShotName);

            TestContext.AddTestAttachment(screenShotFile, screenShotDescription);
        }
    }
}
