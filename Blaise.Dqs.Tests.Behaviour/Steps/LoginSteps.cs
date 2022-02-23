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
        private readonly ScenarioContext _scenarioContext;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void SetupFeature()
        {
            var userModel = new UserModel
            {
                UserName = $"BDSS-test-user-{Guid.NewGuid()}",
                Password = $"{Guid.NewGuid()}",
                Role = "BDSS",
                ServerParks = new List<string> { BlaiseConfigurationHelper.ServerParkName },
                DefaultServerPark = BlaiseConfigurationHelper.ServerParkName
            };

            _scenarioContext.Set(userModel, "userModel");
            UserHelper.GetInstance().CreateUser(userModel);
        }

        [Given(@"I am a BDSS user")]
        public void GivenIAmABdssUser()
        {
        }

        [Given(@"I have logged into to DQS")]
        public void GivenIHaveLoggedIntoToDqs()
        {
            var userModel = _scenarioContext.Get<UserModel>("userModel");
            LogInToDqs(userModel);
        }

        private void LogInToDqs(UserModel userModel)
        {
            DqsHelper.GetInstance().LoginToDqs(userModel.UserName, userModel.Password);
        }

        private void LogOutOfDqs()
        {
            DqsHelper.GetInstance().LogOutOfDqs();
        }

        [AfterScenario]
        public void CleanUpFeature()
        {
            var userModel = _scenarioContext.Get<UserModel>("userModel");
            UserHelper.GetInstance().RemoveUser(userModel.UserName);

            //LogOutOfDqs();
        }
    }
}
