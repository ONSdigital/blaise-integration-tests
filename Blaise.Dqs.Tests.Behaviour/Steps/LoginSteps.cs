namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    using System;
    using System.Collections.Generic;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Dqs;
    using Blaise.Tests.Helpers.User;
    using Blaise.Tests.Models.User;
    using Reqnroll;

    [Binding]
    public sealed class LoginSteps
    {
        private static readonly string _username = $"BDSS-test-user-{Guid.NewGuid()}";
        private static readonly string _password = $"{Guid.NewGuid()}";

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var userModel = new UserModel
            {
                Username = _username,
                Password = _password,
                Role = "BDSS",
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName,
            };
            UserHelper.GetInstance().CreateUser(userModel);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            UserHelper.GetInstance().RemoveUser(_username);
            BrowserHelper.ClearSessionData();
        }

        [Given(@"I am a BDSS user")]
        public void GivenIAmABdssUser()
        {
        }

        [Given(@"I have logged into DQS")]
        public void GivenIHaveLoggedIntoDqs()
        {
            DqsHelper.GetInstance().LogIntoDqs(_username, _password);
        }
    }
}
