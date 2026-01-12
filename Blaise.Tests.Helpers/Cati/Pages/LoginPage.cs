using System;
using System.Configuration;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Framework;
using OpenQA.Selenium;

namespace Blaise.Tests.Helpers.Cati.Pages
{
    public class LoginPage : BasePage
    {
        private readonly string _blaiseVersion =
            ConfigurationManager.AppSettings["ENV_BLAISE_VERSION"];

        private const string _loginButtonPath = "//button[@type='submit']";

        public LoginPage()
          : base(CatiConfigurationHelper.LoginUrl)
        {
        }

        private string UsernameBoxId
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "Username";

                    case "v16":
                        return "qa_username";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        private string PasswordBoxId
        {
            get
            {
                switch (_blaiseVersion?.ToLowerInvariant())
                {
                    case "v14":
                        return "Password";

                    case "v16":
                        return "qa_password";

                    default:
                        throw new ConfigurationErrorsException(
                            $"Unsupported ENV_BLAISE_VERSION value: '{_blaiseVersion}'. Expected 'v14' or 'v16'.");
                }
            }
        }

        /// <inheritdoc/>
        protected override Func<IWebDriver, bool> PageHasLoaded()
        {
            // v14 didn't have a reliable page load hook, v16 does —
            // this works safely for both
            return driver => driver.FindElement(By.XPath(_loginButtonPath)) != null;
        }

        /// <summary>
        /// Logs into the CATI dashboard using the specified username and password.
        /// </summary>
        public void LoginToCati(string username, string password)
        {
            PopulateInputById(UsernameBoxId, username);
            PopulateInputById(PasswordBoxId, password);
            ClickButtonByXPath(_loginButtonPath);
        }
    }
}
