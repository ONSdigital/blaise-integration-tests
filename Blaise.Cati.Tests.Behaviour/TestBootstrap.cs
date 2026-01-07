using Blaise.Tests.Helpers.Cati;
using Blaise.Tests.Helpers.Cati.Pages;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Helpers.Configuration.Interfaces;
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
        Container.Register<LoginPageV16>();
        Container.Register<Blaise.Tests.Helpers.Cati.Pages.LoginPage>();

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

        Container.Register<CatiConfigurationHelper>();
        Container.Register<CatiConfigurationHelperV16>();


        //TODO: Refactor to use shared version resolver class so you don't have loads of if statements everywhere
        Container.Register<ICatiConfigurationHelper>(() =>
        {
            var version = ConfigurationManager.AppSettings["ENV_BLAISE_VERSION"];

            switch (version?.ToLowerInvariant())
            {
                case "v16":
                    return Container.GetInstance<CatiConfigurationHelperV16>();

                case "v14":
                default:
                    return Container.GetInstance<CatiConfigurationHelper>();
            }
        });


        Container.Register<CatiManagementHelper>();


        Container.Verify();
    }
}
