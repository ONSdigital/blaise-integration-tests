using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class LoginPage : BasePage
    {
        private readonly string _usernameBoxId = "Username";
        private readonly string _passwordBoxId = "Password";
        private readonly string _loginButtonPath = "//button[@type='submit']";

        public LoginPage() : base(CatiConfigurationHelper.LoginUrl)
        {
        }
        
        public void LoginToCati(string username, string password)
        {
            PopulateTextboxById(_usernameBoxId, username);
            PopulateTextboxById(_passwordBoxId, password);
            ClickButtonByXPath(_loginButtonPath);
        }
    }
}
