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
        private readonly ScenarioContext _scenarioContext;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario()]
        public void SetupFeature()
        {
            var userModel = new UserModel
            {
                UserName = $"DQS-{Guid.NewGuid()}",
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
        public void GivenIHaveLoggedIntoToDQS()
        {
            var userModel = _scenarioContext.Get<UserModel>("userModel");
            LogInToDqs(userModel);
        }

        private void LogInToDqs(UserModel userModel)
        {
            try
            {
                DqsHelper.GetInstance().LoginToDqs(userModel.UserName, userModel.Password);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "LogIntoDqs", "Log into DQS");
            }
        }

        private void LogOutOfDqs()
        {
            try
            {
                DqsHelper.GetInstance().LogOutOfDqs();
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "LogOutOfDqs", "Log out of DQS");
            }
        }

        private static void FailWithScreenShot(Exception e, string screenShotName, string screenShotDescription)
        {
            var screenShotFile = BrowserHelper.TakeScreenShot(TestContext.CurrentContext.WorkDirectory,
                screenShotName);

            TestContext.AddTestAttachment(screenShotFile, screenShotDescription);
            Assert.Fail($"The test failed to complete - {e.Message}");
        }



        [AfterScenario]
        public void CleanUpFeature()
        {
            LogOutOfDqs();
            var userModel = _scenarioContext.Get<UserModel>("userModel");
            UserHelper.GetInstance().RemoveUser(userModel.UserName);
        }
    }
}
