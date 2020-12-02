using Blaise.Tests.Helpers.Configuration;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class LoginPage : BasePage
    {
        private const string UsernameBoxId = "Username";
        private const string PasswordBoxId = "Password";
        private const string LoginButtonPath = "//button[@type='submit']";

        public LoginPage() : base(CatiConfigurationHelper.LoginUrl)
        {
        }
        
        public void LoginToCati(string username, string password)
        {
            PopulateTextBoxById(UsernameBoxId, username);
            PopulateTextBoxById(PasswordBoxId, password);
            ClickButtonByXPath(LoginButtonPath);
        }
    }
}
