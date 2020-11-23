using Blaise.Nuget.Api.Api;
using Blaise.Smoke.Tests.Helpers;
using Blaise.Smoke.Tests.Models;
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
        private IEnumerable<CaseModel> casesToInstall;
        private UserModel userToCreate;
        private readonly ConfigurationHelper _configurationHelper;
        private readonly InstrumentHelper _instrumentHelper;
        private readonly CaseHelper _caseHelper;
        private readonly UserHelper _userHelper;
        private readonly SeleniumHelper _seleniumHelper;

        public SmokeTestSteps()
        {
            _configurationHelper = new ConfigurationHelper();
            _instrumentHelper = new InstrumentHelper();
            _caseHelper = new CaseHelper();
            _userHelper = new UserHelper();
            _seleniumHelper = new SeleniumHelper();
        }

        [Given(@"I have an instrument we wish to use")]
        public void GivenIHaveAnInstrumentWeWishToUse()
        {
            _instrumentPath = _configurationHelper.InsturmentPath;
        }

        [Given(@"we have a sample set of cases we wish to use")]
        public void GivenWeHaveASampleSetOfCasesWeWishToUse(IEnumerable<CaseModel> cases)
        {        
            casesToInstall = cases;
        }

        [Given(@"I have a new user with the following details")]
        public void GivenIHaveANewUserWithTheFollowingDetails(UserModel userModel)
        {
            if (_userHelper.GetUser(userModel.UserName) != null)
            {
                _userHelper.RemoveUser(userModel.UserName);
            }
            userToCreate = userModel;
        }

        [Given(@"an instrument is available with a sample case \(s\)")]
        public void GivenAnInstrumentIsAvailableWithASampleCaseS()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I have a case ready for data capture")]
        public void GivenIHaveACaseReadyForDataCapture()
        {
            _seleniumHelper.CheckDayBatchEnteries();
        }

        [When(@"I access the case information")]
        public void WhenIAccessTheCaseInformation()
        {
            _seleniumHelper.LoadCaseInformation();
        }


        [When(@"I upload the instrument")]
        public void WhenIUploadTheInstrument()
        {
            _instrumentHelper.InstallInstrument(_instrumentPath);
        }

        [When(@"I create the cases in the instrument")]
        public void WhenICreateTheCasesInTheInstrument()
        {
            _caseHelper.CreateCases(_instrumentName, casesToInstall);
        }

        [When(@"I create the user account")]
        public void WhenICreateTheUserAccount()
        {
            _userHelper.CreateUser(userToCreate);
        }

        [When(@"I create a daybatch")]
        public void WhenICreateADaybatch()
        {
            _seleniumHelper.CreateDayBatch();
        }

        [Then(@"the instrument is available for use")]
        public void ThenTheInstrumentIsAvailableForUse()
        {
            Assert.IsTrue(_instrumentHelper.CheckInstrumentIsInstalled(_instrumentName));
        }

        [Then(@"cases are present in the db")]
        public void ThenCasesArePresentInTheDb(IEnumerable<CaseModel> specificCases)
        {
            Assert.IsTrue(_caseHelper.GetCasesFromAnInstrument(_instrumentName, specificCases));
        }

        [Then(@"the cases are ready for data capture")]
        public void ThenTheCasesAreReadyForDataCapture()
        {
            var enteries = _seleniumHelper.CheckDayBatchEnteries();
            Assert.AreEqual("Showing 1 to 2 of 2 entries", enteries);
        }


        [Then(@"The user is created with the following details")]
        public void ThenTheUserIsCreatedWithTheFollowingDetails()
        {
            var user = _userHelper.GetUser(userToCreate.UserName);
            Assert.IsNotNull(user);
        }

        [Then(@"I am presented with the data capture screen for the case")]
        public void ThenIAmPresentedWithTheDataCaptureScreenForTheCase()
        {
            Assert.NotNull(_seleniumHelper.AccessCase());
        }

    }
}
