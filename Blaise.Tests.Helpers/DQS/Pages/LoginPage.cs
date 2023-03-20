﻿using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using System.Threading;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class LoginPage : BasePage
    {
        private readonly string _usernameTextBoxName = "Username";
        private readonly string _passwordTextBoxName = "Password";
        private readonly string _submitButtonPath = "//button[@type='submit']";
        private readonly string _signOutId = "signout-button";

        public LoginPage() : base(DqsConfigurationHelper.DqsUrl)
        {
        }

        public void LogIntoDqs(string username, string password)
        {
            PopulateInputByName(_usernameTextBoxName, username);
            PopulateInputByName(_passwordTextBoxName, password);
            ClickButtonByXPath(_submitButtonPath);
            Thread.Sleep(2000); /*needed to allow UI to catch up*/
        }

        public void LogOutOfDqs()
        {
            ClickButtonById(_signOutId);
        }
    }
}
