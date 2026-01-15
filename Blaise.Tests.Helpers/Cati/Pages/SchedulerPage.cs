using System;
using System.Configuration;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class SchedulerPage : BasePage
    {
        private readonly string _blaiseVersion =
            ConfigurationManager.AppSettings["ENV_BLAISE_VERSION"];

        private const string _usernameTextBoxName = "name";
        private const string _passwordTextBoxName = "password";
        private const string _submitButtonPath = "//input[@type='submit']";

        public SchedulerPage()
            : base(
                CatiConfigurationHelper.SchedulerUrl,
                GetLayoutParameter())
        {
        }

        private static string GetLayoutParameter()
        {
            var version = ConfigurationManager.AppSettings["ENV_BLAISE_VERSION"];

            switch (version?.ToLowerInvariant())
            {
                case "v14":
                    return "LayoutSet=CATI-Interviewer_Large";

                case "v16":
                    return null; // no layout parameter required

                default:
                    throw new ConfigurationErrorsException(
                        $"Unsupported ENV_BLAISE_VERSION value: '{version}'. Expected 'v14' or 'v16'.");
            }
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
