﻿using Blaise.Tests.Helpers.Browser;
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
        private static readonly string Username = $"BDSS-test-user-{Guid.NewGuid()}";
        private static readonly string Password = $"{Guid.NewGuid()}";

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var userModel = new UserModel
            {
                Username = Username,
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

        [Given(@"I have logged into DQS")]
        public void GivenIHaveLoggedIntoDqs()
        {
            DqsHelper.GetInstance().LogIntoDqs(Username, Password);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            UserHelper.GetInstance().RemoveUser(Username);
            BrowserHelper.ClearSessionData();
        }
    }
}
