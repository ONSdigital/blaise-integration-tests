using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Dqs.Pages;
using SimpleInjector;
using System.Configuration;
using TechTalk.SpecFlow;

[Binding]
public static class TestBootstrap
{
    public static readonly Container Container = new Container();

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        // STEP 2A: Register both versions of the LoginPage
        Container.Register<LoginPageV16>();
        Container.Register<Blaise.Tests.Helpers.Cati.Pages.LoginPage>(); // Rename original LoginPage to LoginPageV2 for clarity

        // STEP 2B: Register ILoginPage and select implementation based on config
        Container.Register<ILoginPage>(() =>
        {
            var version = ConfigurationManager.AppSettings["ENV_BLAISE_VERSION"];

            switch (version?.ToLowerInvariant())
            {
                case "v16":
                    return Container.GetInstance<LoginPageV16>();
                case "v14":
                default:
                    return Container.GetInstance<Blaise.Tests.Helpers.Cati.Pages.LoginPage>();
            }
        });

        // STEP 2C: Register the CatiManagementHelper which depends on ILoginPage
        Container.Register<CatiManagementHelper>();

        // Optional: Verify container config
        Container.Verify();
    }
}
