using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Tests.Helpers.Configuration;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class InterviewLoginPage : BasePage
    {
        private readonly string _usernameTextboxName = "name";
        private readonly string _passwordTextboxName = "password";
        private readonly string _submitButtonPath = "//input[@type='submit']";

        public InterviewLoginPage(IWebDriver driver) : base(driver, CatiConfigurationHelper.InterviewUrl)
        {
        }

        public void LogIntoInterviewPortal(string username, string password)
        {
            PopulateTextboxByName(_usernameTextboxName, username);
            PopulateTextboxByName(_passwordTextboxName, password);
            ClickButtonByXPath(_submitButtonPath);
        }
    }
}
