using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Dqs.Pages
{
    public class LoginPage : BasePage
    {
        private readonly string _usernameTextBoxName = "Username";
        private readonly string _passwordTextBoxName = "Password";
        private readonly string _submitButtonPath = "//button[@type='submit']";
        private readonly string _signOutButtonPath = "//button/span[contains(text(),'Sign out')]";


        public LoginPage() : base(DqsConfigurationHelper.DqsUrl)
        {
        }

        public void LogIntoDqs(string username, string password)
        {
            PopulateInputByName(_usernameTextBoxName, username);
            PopulateInputByName(_passwordTextBoxName, password);
            ClickButtonByXPath(_submitButtonPath);
        }

        public void LogOutOfDqs()
        {
            ClickButtonByXPath(_signOutButtonPath);
        }
    }
}
