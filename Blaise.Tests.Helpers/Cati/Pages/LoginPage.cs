using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

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
            PopulateInputById(UsernameBoxId, username);
            PopulateInputById(PasswordBoxId, password);
            ClickButtonByXPath(LoginButtonPath);
        }
    }
}
