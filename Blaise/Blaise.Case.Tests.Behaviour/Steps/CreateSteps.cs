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
        [BeforeFeature("case")]
        public static void InitializeFeature()
        {
            InstrumentHelper.GetInstance().InstallInstrument();
        }

        [Given(@"I have created sample cases for the instrument")]
        [When(@"I create sample cases for the instrument")]
        public void WhenICreateACaseForTheInstrument(IEnumerable<CaseModel> caseModels)
        {
            CaseHelper.GetInstance().CreateCases(caseModels);
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
            var actualNumberOfCases = CaseHelper.GetInstance().NumberOfCasesInBlaise();
            
            if (expectedNumberOfCases != actualNumberOfCases)
            {
                Assert.Fail($"Expected '{expectedNumberOfCases}' cases in Blaise, but {actualNumberOfCases} cases were found");
            }
        }

        private void CheckCasesMatch(IEnumerable<CaseModel> expectedCases)
        {
            var actualCases = CaseHelper.GetInstance().GetCasesInBlaise().ToList();
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

        [AfterScenario("case")]
        public void CleanUpScenario()
        {
            //CaseHelper.GetInstance().DeleteCases();
        }

        [AfterFeature("case")]
        public static void CleanUpFeature()
        {
           // InstrumentHelper.GetInstance().UninstallSurvey();
        }
    }
}
