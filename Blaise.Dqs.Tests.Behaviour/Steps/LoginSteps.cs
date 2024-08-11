using Blaise.Tests.Helpers.Browser;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.User;
using Blaise.Tests.Models.User;
using System;
using System.Collections.Generic;
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
            DqsHelper.GetInstance().LogInToDqs(UserName, Password);
        }

        [AfterTestRun]
        public static void CleanUpTestRun()
        {
            UserHelper.GetInstance().RemoveUser(UserName);
            BrowserHelper.ClearSessionData();
        }
    }
}
