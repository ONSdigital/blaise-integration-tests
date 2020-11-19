using Blaise.Nuget.Api.Api;
using Blaise.Smoke.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Blaise.Smoke.Tests.Steps
{
    [Binding]
    public sealed class SmokeTestSteps
    {
        private string _instrumentName = "OPN2101A";
        private string _instrumentPath;
        private readonly ConfigurationHelper _configurationHelper;
        private readonly InstrumentHelper _instrumentHelper;

        public SmokeTestSteps()
        {
            _configurationHelper = new ConfigurationHelper();
            _instrumentHelper = new InstrumentHelper();
        }

        [Given(@"I have an instrument we wish to use")]
        public void GivenIHaveAnInstrumentWeWishToUse()
        {
            _instrumentPath = _configurationHelper.InsturmentPath;
        }

        [When(@"I upload the instrument")]
        public void WhenIUploadTheInstrument()
        {
            _instrumentHelper.InstallInstrument(_instrumentPath);
        }

        [Then(@"the instrument is available for use")]
        public void ThenTheInstrumentIsAvailableForUse()
        {
            Assert.IsTrue(_instrumentHelper.CheckInstrumentIsInstalled(_instrumentName));
        }


    }
}
