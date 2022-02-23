using System;
using System.Collections.Generic;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Dqs;
using Blaise.Tests.Helpers.User;
using Blaise.Tests.Models.User;
using TechTalk.SpecFlow;

namespace Blaise.Dqs.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class LoginSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly string _userName = $"BDSS-test-user-{Guid.NewGuid()}";
        private readonly string _password = $"{Guid.NewGuid()}";

        [BeforeTestRun]
        public void SetupFeature()
        {
            var userModel = new UserModel
            {
                UserName = _userName,
                Password = _password,
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
            DqsHelper.GetInstance().LoginToDqs(_userName, _password);
        }

        [AfterTestRun]
        public void CleanUp()
        {
            UserHelper.GetInstance().RemoveUser(_userName);
        }
    }
}
