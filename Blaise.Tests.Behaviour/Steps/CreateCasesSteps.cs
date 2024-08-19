﻿using System.Collections.Generic;
using System.Linq;
using Blaise.Tests.Helpers.Case;
using Blaise.Tests.Helpers.Questionnaire;
using Blaise.Tests.Helpers.Configuration;
using Blaise.Tests.Models.Case;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Blaise.Tests.Behaviour.Steps
{
    [Binding]
    public sealed class CreateCasesSteps
    {
        [Given(@"I have created sample cases for the questionnaire")]
        [When(@"I create cases for the questionnaire")]
        public void WhenICreateCasesForTheQuestionnaire(IEnumerable<CaseModel> caseModels)
        {
            CaseHelper.GetInstance().DeleteCases();
            CaseHelper.GetInstance().CreateCases(caseModels);
        }

        [Then(@"the cases are available in the questionnaire")]
        public void ThenTheCasesAreAvailableInTheQuestionnaire(IEnumerable<CaseModel> cases)
        {
            var expectedCases = cases.ToList();
            CheckNumberOfCasesMatch(expectedCases.Count);
            CheckCasesMatch(expectedCases);
        }

        private void CheckNumberOfCasesMatch(int expectedNumberOfCases)
        {
            var actualNumberOfCases = CaseHelper.GetInstance().NumberOfCasesInQuestionnaire();
            
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

        [AfterScenario("create-casess")]
        public void AfterScenario()
        {
            CaseHelper.GetInstance().DeleteCases();
        }

        [AfterFeature("create-casess")]
        public static void AfterFeature()
        {
            QuestionnaireHelper.GetInstance().UninstallQuestionnaire(BlaiseConfigurationHelper.QuestionnaireName, BlaiseConfigurationHelper.ServerParkName);
        }
    }
}
