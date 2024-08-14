using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SchedulerPage : BasePage
    {
        private readonly string _usernameTextBoxName = "name";
        private readonly string _passwordTextBoxName = "password";
        private readonly string _submitButtonPath = "//input[@type='submit']";

        public SchedulerPage() : base(CatiConfigurationHelper.SchedulerUrl, "LayoutSet=CATI-Interviewer_Large")
        {
        }

        public void LogIntoScheduler(string username, string password)
        {
            PopulateInputByName(_usernameTextBoxName, username);
            PopulateInputByName(_passwordTextBoxName, password);
            ClickButtonByXPath(_submitButtonPath);
        }

        public void LoadPageForSpecificCase(string url)
        {
            LoadSpecificPage(url);
        }

        public void LoginButtonIsAvailable()
        {
            ButtonIsAvailableByPath(_submitButtonPath);
        }
    }
}
