using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class InterviewLoginPage : BasePage
    {
        private readonly string _usernameTextBoxName = "name";
        private readonly string _passwordTextBoxName = "password";
        private readonly string _submitButtonPath = "//input[@type='submit']";

        public InterviewLoginPage() : base(CatiConfigurationHelper.InterviewUrl)
        {
        }

        public void LogIntoInterviewPortal(string username, string password)
        {
            PopulateInputByName(_usernameTextBoxName, username);
            PopulateInputByName(_passwordTextBoxName, password);
            ClickButtonByXPath(_submitButtonPath);
        }

        public void LoginButtonIsAvailable()
        {
            ButtonIsAvailableByPath(_submitButtonPath);
        }
    }
}
