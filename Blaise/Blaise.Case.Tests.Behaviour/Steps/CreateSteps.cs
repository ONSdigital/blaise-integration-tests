using System.Collections.Generic;
using System.Linq;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Instrument;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Case.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class CreateSteps
    {
        private readonly InstrumentHelper _instrumentHelper;
        private readonly CaseHelper _caseHelper;

        public CreateSteps()
        {
            _instrumentHelper = new InstrumentHelper();
            _caseHelper = new CaseHelper();
        }

        [BeforeFeature]
        public static void InstallInstrument()
        {
            InstrumentHelper.CreateInstance().InstallInstrument();
        }

        [Given(@"I have an instrument installed on a Blaise environment")]
        public void GivenIHaveAnInstrumentInstalledOnABlaiseEnvironment()
        {
            _instrumentHelper.SurveyHasInstalled();
        }
        
        [When(@"I create sample cases for the instrument")]
        public void WhenICreateACaseForTheInstrument(IEnumerable<CaseModel> caseModels)
        {
            _caseHelper.CreateCases(caseModels);
        }

        [Then(@"the sample cases are available in the Blaise environment")]
        public void ThenTheCaseIsAvailableInTheBlaiseEnvironment(IEnumerable<CaseModel> cases)
        {
            var expectedCases = cases.ToList();
            CheckNumberOfCasesMatch(expectedCases.Count);
            CheckCasesMatch(expectedCases);
        }

        private void CheckNumberOfCasesMatch(int expectedNumberOfCases)
        {
            var actualNumberOfCases = _caseHelper.NumberOfCasesInBlaise();
            
            if (expectedNumberOfCases != actualNumberOfCases)
            {
                Assert.Fail($"Expected '{expectedNumberOfCases}' cases in Blaise, but {actualNumberOfCases} cases were found");
            }
        }

        private void CheckCasesMatch(IEnumerable<CaseModel> expectedCases)
        {
            var actualCases = _caseHelper.GetCasesInBlaise().ToList();
            foreach (var expectedCase in expectedCases)
            {
                var actualCase = actualCases.FirstOrDefault(c => c.PrimaryKey == expectedCase.PrimaryKey);

                if (actualCase == null)
                {
                    Assert.Fail($"Case '{expectedCase.PrimaryKey}' was not found in Blaise");
                }

                Assert.True(actualCase.Equals(expectedCase), $"Case '{expectedCase.PrimaryKey}' did not match the case in Blaise");
            }
        }

        [AfterScenario]
        public void CleanUp()
        {
            _caseHelper.DeleteCases();
        }

        [AfterFeature]
        public static void UnInstallInstrument()
        {
            InstrumentHelper.CreateInstance().UninstallSurvey();
        }
    }
}
