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

        [Given(@"I am a BDSS user")]
        public void GivenIAmABdssUser()
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

        [Given(@"I have logged into to DQS")]
        public void GivenIHaveLoggedIntoToDQS()
        {
            try
            {
                var userModel = _scenarioContext.Get<UserModel>("userModel");
                DqsHelper.GetInstance().LoginToDqs(userModel.UserName, userModel.Password);
            }
            catch (Exception e)
            {
                FailWithScreenShot(e, "GivenIHaveLoggedIntoToDQS", "Log into DQS");
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
            DqsHelper.GetInstance().LogOutOfDqs();
            var userModel = _scenarioContext.Get<UserModel>("userModel");
            UserHelper.GetInstance().RemoveUser(userModel.UserName);
        }
    }
}
