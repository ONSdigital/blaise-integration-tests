namespace Blaise.Tests.Helpers.Cati
{
    using System;
    using Blaise.Tests.Helpers.Browser;
    using Blaise.Tests.Helpers.Configuration;
    using Blaise.Tests.Helpers.Framework.Extensions;

    public enum CatiUiVersion
    {
        Unknown = 0,
        LegacyDashboard = 1,
        NewDashboard = 2,
    }

    public static class CatiUiVersionHelper
    {
        private const string NewUiIconXPath = "//i[contains(@class, 'bi-bell-fill')]";
        private static CatiUiVersion _currentVersion = CatiUiVersion.Unknown;

        public static bool IsDetected => _currentVersion != CatiUiVersion.Unknown;

        public static CatiUiVersion CurrentVersion
        {
            get
            {
                EnsureDetected();
                return _currentVersion;
            }
        }

        public static bool IsNewUi
        {
            get
            {
                EnsureDetected();
                return _currentVersion == CatiUiVersion.NewDashboard;
            }
        }

        public static void Reset()
        {
            _currentVersion = CatiUiVersion.Unknown;
        }

        public static void DetectAndCache()
        {
            if (IsDetected)
            {
                Console.WriteLine($"CATI UI version already detected: {_currentVersion}.");
                return;
            }

            var overrideValue = ConfigurationExtensions.TryGetVariable("ENV_BLAISE_CATI_UI_VERSION");
            if (!string.IsNullOrWhiteSpace(overrideValue))
            {
                _currentVersion = ParseOverride(overrideValue);
                Console.WriteLine($"CATI UI version overridden by ENV_BLAISE_CATI_UI_VERSION: {_currentVersion}.");
                return;
            }

            Console.WriteLine("Detecting CATI UI version...");

            BrowserHelper.NavigateToPage(CatiConfigurationHelper.LoginUrl);
            BrowserHelper
                .Wait("Timed out waiting for CATI login page to load")
                .Until(driver =>
                    driver.Url.IndexOf(CatiConfigurationHelper.LoginUrl, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    driver.Url.IndexOf(CatiConfigurationHelper.NewDashboardLoginUrl, StringComparison.OrdinalIgnoreCase) >= 0);

            var currentUrl = BrowserHelper.CurrentUrl ?? string.Empty;
            var isNewUi = currentUrl.IndexOf(
                    CatiConfigurationHelper.NewDashboardLoginUrl,
                    StringComparison.OrdinalIgnoreCase) >= 0 ||
                BrowserHelper.ElementExistsByXPath(NewUiIconXPath, TimeSpan.FromSeconds(3));

            if (!isNewUi)
            {
                BrowserHelper.NavigateToPage(CatiConfigurationHelper.NewDashboardLoginUrl);
                BrowserHelper
                    .Wait("Timed out waiting for new CATI login page to load")
                    .Until(driver =>
                        driver.Url.IndexOf(
                            CatiConfigurationHelper.NewDashboardLoginUrl,
                            StringComparison.OrdinalIgnoreCase) >= 0);
                isNewUi = BrowserHelper.ElementExistsByXPath(NewUiIconXPath, TimeSpan.FromSeconds(3));
            }

            _currentVersion = isNewUi ? CatiUiVersion.NewDashboard : CatiUiVersion.LegacyDashboard;
            Console.WriteLine($"Detected CATI UI version: {_currentVersion}.");
        }

        private static CatiUiVersion ParseOverride(string overrideValue)
        {
            var normalized = overrideValue.Trim().ToLowerInvariant();
            if (normalized == "new" || normalized == "newdashboard" || normalized == "dashboard")
            {
                return CatiUiVersion.NewDashboard;
            }

            if (normalized == "old" || normalized == "legacy" || normalized == "classic")
            {
                return CatiUiVersion.LegacyDashboard;
            }

            throw new ArgumentException(
                $"Invalid ENV_BLAISE_CATI_UI_VERSION value '{overrideValue}'. Expected 'new' or 'old'.");
        }

        private static void EnsureDetected()
        {
            if (!IsDetected)
            {
                throw new InvalidOperationException(
                    "CATI UI version has not been detected. Call CatiUiVersionHelper.DetectAndCache() before using selectors.");
            }
        }
    }
}
